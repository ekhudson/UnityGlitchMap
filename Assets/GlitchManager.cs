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
	
	public string ImageLocation = string.Empty;
	
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
		
		LoadImage(ImageLocation);
		
	}
	
	void LoadImage(string location)
	{
		if (string.IsNullOrEmpty(location))
		{
			location = "file://" + Application.dataPath + "/Resources/lance.jpg";	
		}	
		
		textureData = new WWW(location);	
		
		GetHeaderSize(textureData.bytes);
		
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
			random = ( random * 16807 ) % 2147483647;			
			
			int pos = (int)(byteArray.Length * random * 4.656612875245797e-10);			
			
			if (i >= byteArray.Length || pos >= byteArray.Length)
			{
				return;
			}
			
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
		
		GUILayout.BeginArea(new Rect(0,0, Screen.width, Screen.height));
		
		
		GUILayout.BeginHorizontal();
		
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginVertical();
		
		GUILayout.FlexibleSpace();
		
		GUILayout.Box(string.Empty, new GUILayoutOption[] {GUILayout.Width(ImageSize), GUILayout.Height(ImageSize)});
		
		Rect rect = GUILayoutUtility.GetLastRect();
		
		GUI.DrawTexture(rect, mGlitchedTexture);
		
		GUILayout.BeginHorizontal();
		
		ImageLocation = GUILayout.TextField(ImageLocation);
		
		if (GUILayout.Button("Load Image"))
		{
			LoadImage(ImageLocation);
		}		
		
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		
		GUILayout.Label("Image Size: ");
		ImageSize = System.Int32.Parse( GUILayout.TextField(Glitchiness.ToString()) );
		ImageSize = (int)GUILayout.HorizontalSlider(Glitchiness, 0, 150);
		
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		
		GUILayout.Label("Glitchiness: ");
		Glitchiness = System.Int32.Parse( GUILayout.TextField(Glitchiness.ToString()) );
		Glitchiness = (int)GUILayout.HorizontalSlider(Glitchiness, 0, 150);
		
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		
		GUILayout.Label("Max Iterations: ");
		MaxIterations = System.Int32.Parse( GUILayout.TextField(MaxIterations.ToString()) );
		MaxIterations = (int)GUILayout.HorizontalSlider(MaxIterations, 1, 5);
		
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		
		GUILayout.Label("Seed: ");
		Seed = System.Int32.Parse( GUILayout.TextField(Seed.ToString()) );
		Seed = (int)GUILayout.HorizontalSlider(Seed, 1, 1000);
		
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		
		GUILayout.EndVertical();
		
		GUILayout.FlexibleSpace();
		
		GUILayout.EndHorizontal();
		
		GUILayout.EndArea();
		
	}
	
	public void GetHeaderSize(byte[] byteArray)
	{	
		    byte[] _bytesSource = byteArray;			
			int _headerSize = 417;
		
			int length = byteArray.Length;
			int count = 0;

			uint b;
			
			while(count < length)
			{
				b = byteArray[count];
				
				if(b == 0xFF)
				{
					b = byteArray[count];
					
					if(b == 0xDA)
					{
						_headerSize = count + (int)b;
						break;
					}
					
					count++;
				}
			}			
	}
}
