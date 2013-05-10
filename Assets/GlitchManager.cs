using UnityEngine;
using System.Collections;


public class GlitchManager : MonoBehaviour 
{
	public Texture2D TextureToUse;	
	public float Glitchiness = 1;
	public int MaxIterations = 1;
	public int Seed = 2341;
	public float Speed = 5;
	
	public int ImageSize = 512;
	
	public string ImageLocation = string.Empty;
	
	private Texture2D mGlitchedTexture;
	
	private WWW textureData;
	
	
	private int mHeaderSize = 500;
	
	private float mCurrentGlitchiness;
	private int mCurrentIterations;
	private int mCurrentSeed;
	private byte[] mByteArray;
	private byte[] mOriginalByteArray;
	private string[] mByteString;
	private string mByteStringCollapsed;

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
		
		while (!textureData.isDone)
		{
			//wait
		}		
		
		mOriginalByteArray = mByteArray = textureData.bytes;
		
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
		
		mCurrentGlitchiness = (float)Mathf.Lerp(mCurrentGlitchiness, Glitchiness, Time.deltaTime * Speed);
		mCurrentIterations = (int)Mathf.Lerp(mCurrentIterations, MaxIterations, Time.deltaTime * Speed);
		mCurrentSeed = (int)Mathf.Lerp(mCurrentSeed, Seed, Time.deltaTime * Speed);			
		
		byte[] byteArray = textureData.bytes;	
		mOriginalByteArray = byteArray;
		
		float random = mCurrentSeed;		
		int length = (byteArray.Length - mHeaderSize - 2);
		
		for(int i = 0; i < (mCurrentGlitchiness * mCurrentIterations); i++)
		{			
			random = ( random * 16807 ) % 2147483647;			
			
			int pos = mHeaderSize + (int)(length * random * 4.656612875245797e-10);			
			
			if (i >= byteArray.Length || pos >= byteArray.Length || pos <= mHeaderSize - 2)
			{
				return;
			}
			
			byteArray[pos] = new byte();			
		}		
		
		mByteArray = byteArray;
		
		mGlitchedTexture.LoadImage(byteArray);
		
		mByteString = new string[mByteArray.Length];
		
		for(int i = 0; i < mByteArray.Length; i++)
		{
			mByteString[i] = mByteArray[i].ToString();
			//mByteString[i] = System.Text.Encoding.Default.GetString(mByteArray, i, 1);
		}
		
		
		mByteStringCollapsed = System.String.Join(string.Empty, mByteString);
	}
	
	void OnGUI()
	{
		if (mGlitchedTexture == null)
		{
			return;
		}		
		
		Rect labelRect = new Rect(0,0,Screen.width,Screen.height);
		
		
		GUI.Label(labelRect, mByteStringCollapsed);		
				
		
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
		
		GUILayout.Label("Glitchiness: ");
		Glitchiness = float.Parse( GUILayout.TextField(Glitchiness.ToString("0.00")) );
		Glitchiness = (float)GUILayout.HorizontalSlider(Glitchiness, 0f, 1f);
		
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		
		GUILayout.Label("Max Iterations: ");
		MaxIterations = System.Int32.Parse( GUILayout.TextField(MaxIterations.ToString()) );
		MaxIterations = (int)GUILayout.HorizontalSlider(MaxIterations, 1, 1024);
		
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
				b = _bytesSource[count];
							
				if(System.Convert.ToByte(b) == 0xFF)
				{
					b = _bytesSource[count];
					
					if(System.Convert.ToByte(b) == 0xDA)
					{
						_headerSize = count + (int)b;
						break;
					}					
				}
			
				count++;
			}			
		
			mHeaderSize = _headerSize;
	}
}
