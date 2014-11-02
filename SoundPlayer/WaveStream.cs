namespace SoundPlayer
{
    using System;
    using System.IO;

    public class WaveStream : Stream
    {
        public override bool CanSeek
        {
            // シークはサポートしない
            get { return false; }
        }

        public override bool CanRead
        {
            get { return !IsClosed; }
        }

        public override bool CanWrite
        {
            // 書き込みはサポートしない
            get { return false; }
        }

        private bool IsClosed
        {
            get { return reader == null; }
        }

        public override long Position
        {
            get { CheckDisposed(); throw new NotSupportedException(); }
            set { CheckDisposed(); throw new NotSupportedException(); }
        }

        public override long Length
        {
            get { CheckDisposed(); throw new NotSupportedException(); }
        }

        public int Volume
        {
            get { CheckDisposed(); return volume; }
            set
            {
                CheckDisposed();

                if (value < 0 || MaxVolume < value)
                {
                    throw new ArgumentOutOfRangeException(
                        "Volume",
                        value,
                        string.Format("0から{0}の範囲の値を指定してください", MaxVolume));
                }

                volume = value;
            }
        }

        public WaveStream(Stream baseStream)
        {
            if (baseStream == null)
                throw new ArgumentNullException("baseStream");
            if (!baseStream.CanRead)
                throw new ArgumentException("読み込み可能なストリームを指定してください", "baseStream");

            this.reader = new BinaryReader(baseStream);

            ReadHeader();
        }

        public override void Close()
        {
            if (reader != null)
            {
                reader.Close();
                reader = null;
            }
        }

        // dataチャンクまでのヘッダブロックの内容をバッファに読み込んでおく
        // WAVEFORMAT等のヘッダ内容のチェックは省略
        private void ReadHeader()
        {
            using (var headerStream = new MemoryStream())
            {
                var writer = new BinaryWriter(headerStream);

                // RIFFヘッダ
                var riffHeader = reader.ReadBytes(12);

                writer.Write(riffHeader);

                // dataチャンクまでの内容をwriterに書き写す
                for (; ; )
                {
                    var chunkHeader = reader.ReadBytes(8);

                    writer.Write(chunkHeader);

                    var fourcc = BitConverter.ToInt32(chunkHeader, 0);
                    var size = BitConverter.ToInt32(chunkHeader, 4);

                    if (fourcc == 0x61746164) // 'data'
                        break;

                    writer.Write(reader.ReadBytes(size));
                }

                writer.Close();

                header = headerStream.ToArray();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            CheckDisposed();

            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", offset, "0以上の値を指定してください");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", count, "0以上の値を指定してください");
            if (buffer.Length - count < offset)
                throw new ArgumentException("配列の範囲を超えてアクセスしようとしました", "offset");

            if (header == null)
            {
                // dataチャンクの読み込み
                // WAVEサンプルを読み込み、音量を適用して返す
                // ストリームは16ビット(1サンプル2バイト)と仮定

                // countバイト以下となるよう読み込むサンプル数を決定する
                var samplesToRead = count / 2;
                var bytesToRead = samplesToRead * 2;
                var len = reader.Read(buffer, offset, bytesToRead);

                if (len == 0)
                    return 0; // 終端まで読み込んだ

                // 読み込んだサンプル1つずつにボリュームを適用する
                for (var sample = 0; sample < samplesToRead; sample++)
                {
                    short s = (short)(buffer[offset] | (buffer[offset + 1] << 8));

                    s = (short)(((int)s * volume) / MaxVolume);

                    buffer[offset] = (byte)(s & 0xff);
                    buffer[offset + 1] = (byte)((s >> 8) & 0xff);

                    offset += 2;
                }

                return len;
            }
            else
            {
                // ヘッダブロックの読み込み
                // バッファに読み込んでおいた内容をそのままコピーする
                var bytesToRead = Math.Min(header.Length - headerOffset, count);

                Buffer.BlockCopy(header, headerOffset, buffer, offset, bytesToRead);

                headerOffset += bytesToRead;

                if (headerOffset == header.Length)
                    // ヘッダブロックを全て読み込んだ
                    // (不要になったヘッダのバッファを解放し、以降はdataチャンクの読み込みに移る)
                    header = null;

                return bytesToRead;
            }
        }

        public override void SetLength(long @value)
        {
            CheckDisposed();

            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            CheckDisposed();

            throw new NotSupportedException();
        }

        public override void Flush()
        {
            CheckDisposed();

            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            CheckDisposed();

            throw new NotSupportedException();
        }

        private void CheckDisposed()
        {
            if (IsClosed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        private BinaryReader reader;
        private byte[] header;
        private int headerOffset = 0;
        private int volume = MaxVolume;
        private const int MaxVolume = 100;
    }
}
