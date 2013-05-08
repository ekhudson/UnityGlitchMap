using UnityEngine;
using System.Collections;


public class GlitchManager : MonoBehaviour 
{
	public Texture2D TextureToUse;	
	public int Glitchiness = 1;
	public int MaxIterations = 1;
	public int Seed = 2341;
	public float Speed = 5;
	
	public int ImageSize = 512;
	
	private Texture2D mGlitchedTexture;
	
	private WWW textureData;
	
	
	private const int kHeaderSize = 500;
	
	private int mCurrentGlitchiness;
	private int mCurrentIterations;
	private int mCurrentSeed;

	// Use this for initialization
	void Start () 
	{
//		string targetFile = "file://" + Application.dataPath + "/lance.jpg";
//		
//		Debug.Log(targetFile);
//		
//		textureData = new WWW(targetFile);
//		mGlitchedTexture = textureData.texture;
//		
//		byte[] byteArray = textureData.bytes;
//		
//		Debug.Log(textureData.bytes.Length);
//		
//		float random = 89320498;		
//		
//		for(int i = kHeaderSize; i < kHeaderSize + Iterations; i++)
//		{
//			random = ( random * 16807 ) % 2147483647;			
//			
//			int pos = (int)(byteArray.Length * random * 4.656612875245797e-10);
//			
//			//Debug.Log ("before: " + byteArray[pos].ToString());
//			
//			//byteArray[pos] = System.BitConverter.GetBytes( Random.Range(0, int.MaxValue) )[0];
//			byteArray[pos] = new byte();
//			
//			//Debug.Log ("after: " + byteArray[pos].ToString());
//		}	
//		
//		
//		mGlitchedTexture.LoadImage(byteArray);
		
		mCurrentGlitchiness = Glitchiness;
		mCurrentIterations = MaxIterations;
		mCurrentSeed = Seed;
		
		
		string targetFile = "file://" + Application.dataPath + "/Resources/lance.jpg";
			
		textureData = new WWW(targetFile);	
		
		mGlitchedTexture = textureData.texture;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (textureData == null || textureData.bytes.Length < 0)
		{
			return;
		}
		
		mCurrentGlitchiness = (int)Mathf.Lerp(mCurrentGlitchiness, Glitchiness, Time.deltaTime * Speed);
		mCurrentIterations = (int)Mathf.Lerp(mCurrentIterations, MaxIterations, Time.deltaTime * Speed);
		mCurrentSeed = (int)Mathf.Lerp(mCurrentSeed, Seed, Time.deltaTime * Speed);	
			
		
		byte[] byteArray = textureData.bytes;		
	
		
		float random = mCurrentSeed;		
		
		for(int i = kHeaderSize; i < kHeaderSize + (mCurrentGlitchiness * mCurrentIterations); i++)
		{
			if (i >= byteArray.Length)
			{
				return;
			}		
			
			random = ( random * 16807 ) % 2147483647;			
			
			int pos = (int)(byteArray.Length * random * 4.656612875245797e-10);			
			
			byteArray[pos] = new byte();		
		}	
		
		
		mGlitchedTexture.LoadImage(byteArray);	
		
	}
	
	void OnGUI()
	{
			if (mGlitchedTexture == null)
		{
			return;
		}
		
		
		GUILayout.BeginHorizontal();
		
		GUILayout.BeginVertical();
		
		
		GUILayout.Box(string.Empty, new GUILayoutOption[] {GUILayout.Width(ImageSize), GUILayout.Height(ImageSize)});
		
		Rect rect = GUILayoutUtility.GetLastRect();
		
		GUI.DrawTexture(rect, mGlitchedTexture);
		
		GUILayout.BeginHorizontal();
		
		GUILayout.Label("Glitchiness: ");
		Glitchiness = (int)GUILayout.HorizontalSlider(Glitchiness, 0, 150);
		
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		
		GUILayout.Label("Max Iterations: ");
		MaxIterations = (int)GUILayout.HorizontalSlider(MaxIterations, 1, 5);
		
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		
		GUILayout.Label("Seed: ");
		Seed = (int)GUILayout.HorizontalSlider(Seed, 1, 1000);
		
		GUILayout.EndHorizontal();
		
		
		GUILayout.EndVertical();
		
		
		GUILayout.EndHorizontal();
	}
	
	public void GetHeaderSize(byte[] byteArray)
	{
	
//		    byte[] _bytesSource = byteArray;			
//			int _headerSize = 417;
//		
//			int length = byteArray.length;
//			int count = 0;
//
//			uint b;
//			
//			while(count < length)
//			{
//				b = byteArray[count];
//				
//				if(b == 0xFF)
//				{
//					b = byteArray[count];
//					
//					if(b == 0xDA)
//					{
//						_headerSize = count + b;
//						break;
//					}
//					
//					count++;
//				}
//			}
//			
//			draw();
	}
}
