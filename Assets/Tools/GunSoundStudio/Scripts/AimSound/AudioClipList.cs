using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AudioClipList : ScriptableObject
{
    public AudioClip[] audioClips;
    public void Load(byte[] data)
    {
        using(var steam = new MemoryStream(data,0,data.Length,false,true))
        {
            using(var reader = new BinaryReader(steam))
            {
                Load(reader,steam);
            }
        }
    }
    public void Load(BinaryReader reader,MemoryStream stream)
    {
        var audioClips = new List<AudioClip>();
        while(stream.Position<stream.Length)
        {
            audioClips.Add(ReadAudio(reader,stream));
        }
        this.audioClips = audioClips.ToArray();
    }

	public static AudioClip ReadAudio(BinaryReader reader,MemoryStream stream)
	{
        // Debug.Log("ReadAudio "+stream.Position);
        var startPosition = stream.Position;
        // chunk 0
        int chunkID       = reader.ReadInt32();
        int fileSize      = reader.ReadInt32();
        // Debug.Log("fileSize "+fileSize);
        int riffType      = reader.ReadInt32();


        // chunk 1
        int fmtID         = reader.ReadInt32();
        int fmtSize       = reader.ReadInt32(); // bytes for this chunk
        int fmtCode       = reader.ReadInt16();
        int channels      = reader.ReadInt16();
        int sampleRate    = reader.ReadInt32();
        int byteRate      = reader.ReadInt32();
        int fmtBlockAlign = reader.ReadInt16();
        int bitDepth      = reader.ReadInt16();

        if (fmtSize == 18)
        {
            // Read any extra values
            int fmtExtraSize = reader.ReadInt16();
            reader.ReadBytes(fmtExtraSize);
        }

        // chunk 2
        int dataID = reader.ReadInt32();
        int bytes = reader.ReadInt32();

        // DATA!
        // byte[] byteArray = reader.ReadBytes(bytes);

        int bytesForSamp = bitDepth/8;
        int samps = bytes / bytesForSamp;


        float[] asFloat = null;
        switch( bitDepth ) {
            case 64:
                double[] asDouble = new double[samps]; 
                asFloat = new float[samps]; 
                Buffer.BlockCopy(stream.GetBuffer(), (int)stream.Position, asDouble, 0, bytes);
                for(int i=0; i<asDouble.Length;++i)
                    asFloat[i] = (float)asDouble[i];
                break;
            case 32:
                asFloat = new float[samps];   
                Buffer.BlockCopy(stream.GetBuffer(), (int)stream.Position, asFloat, 0, bytes);
                break;
            case 16:
                Int16[] asInt16 = new Int16[samps];   
                asFloat = new float[samps]; 
                Buffer.BlockCopy(stream.GetBuffer(), (int)stream.Position, asInt16, 0, bytes);
                for(int i=0; i<asInt16.Length;++i)
                    asFloat[i] = asInt16[i] / (float)Int16.MaxValue ;
                break;
        }
        stream.Position = startPosition + fileSize;
		AudioClip audioClip = AudioClip.Create ("", asFloat.Length/channels,channels, sampleRate, false);
        audioClip.hideFlags = HideFlags.DontSave;
		audioClip.SetData(asFloat, 0);
		return audioClip;
	}
}