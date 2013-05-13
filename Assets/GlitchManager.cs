using UnityEngine;
using System.Collections;


public class GlitchManager : MonoBehaviour 
{
	public Texture2D TextureToUse;	
	public float Glitchiness = 1;
	public int MaxIterations = 1;
	public int Seed = 2341;
	public float Speed = 5;
	public int ByteValue = 0;
	public bool ShowData = false;
	
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
	private string[] mBytesAlteredArray;
	private string mBytesAlteredCollapsed;
	
	
	
	private GUIStyle mLabelStyle;
	

	// Use this for initialization
	void Start () 
	{		
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
		mOriginalByteArray = new byte[byteArray.Length];
		mBytesAlteredArray = new string[byteArray.Length];
		
		for (int j = 0; j < mBytesAlteredArray.Length; j++)
		{
			mOriginalByteArray[j] = byteArray[j];
			mBytesAlteredArray[j] = System.Text.Encoding.Default.GetString(mOriginalByteArray, j, 1);
		}
				
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
			
			byteArray[pos] = (byte)ByteValue;			
		}		
		
		mByteArray = byteArray;
		
		mGlitchedTexture.LoadImage(byteArray);
		
		mByteString = new string[mByteArray.Length];
		
		
		for(int i = 0; i < mByteArray.Length; i++)
		{
			//mByteString[i] = mByteArray[i].ToString();
			mByteString[i] = System.Text.Encoding.Default.GetString(mByteArray, i, 1);
		}
		
		
		mByteStringCollapsed = System.String.Join(" ", mByteString);
		mBytesAlteredCollapsed = System.String.Join(" ", mBytesAlteredArray);	
		
	}
	
	void OnGUI()
	{
		if (mGlitchedTexture == null)
		{
			return;
		}
		
		if (ShowData)
		{
		
			mLabelStyle = new GUIStyle(GUI.skin.label);
			mLabelStyle.wordWrap = true;
			
			Rect labelRect = new Rect(0,0,Screen.width,Screen.height);		
			
			GUI.color = Color.red;	
			
			GUI.Label(labelRect, mBytesAlteredCollapsed, mLabelStyle);	
			
			GUI.color = Color.white;
			
			GUI.Label(labelRect, mByteStringCollapsed, mLabelStyle);				
			
			GUI.color = Color.white;
		}
		
		GUILayout.BeginArea(new Rect(0,0, Screen.width, Screen.height));		
		
		GUILayout.BeginHorizontal();
		
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginVertical();
		
		GUILayout.FlexibleSpace();
		
		GUILayout.Box(string.Empty, new GUILayoutOption[] {GUILayout.Width(ImageSize), GUILayout.Height(ImageSize)});
		
		Rect rect = GUILayoutUtility.GetLastRect();
		
		GUI.DrawTexture(rect, mGlitchedTexture);
		
		GUILayout.BeginVertical(GUI.skin.button,GUILayout.Width(ImageSize));
		
		GUILayout.BeginHorizontal();
		
		ImageLocation = GUILayout.TextField(ImageLocation, GUILayout.Width(ImageSize * 0.75f));
		
		if (GUILayout.Button("Load Image"))
		{
			LoadImage(ImageLocation);
		}		
		
		GUILayout.EndHorizontal();		
		
		GUILayout.BeginHorizontal();
		
		GUILayout.Label("Glitchiness: ", GUI.skin.button);
		Glitchiness = float.Parse( GUILayout.TextField(Glitchiness.ToString("0.00")) );
		Glitchiness = (float)GUILayout.HorizontalSlider(Glitchiness, 0f, 1f);
		
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		
		GUILayout.Label("Max Iterations: ", GUI.skin.button);
		MaxIterations = System.Int32.Parse( GUILayout.TextField(MaxIterations.ToString()) );
		MaxIterations = (int)GUILayout.HorizontalSlider(MaxIterations, 1, 1024);
		
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		
		GUILayout.Label("Seed: ", GUI.skin.button);
		Seed = System.Int32.Parse( GUILayout.TextField(Seed.ToString()) );
		Seed = (int)GUILayout.HorizontalSlider(Seed, 1, 1000);
		
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		
		GUILayout.Label("Byte Value: ", GUI.skin.button);
		ByteValue = System.Int32.Parse( GUILayout.TextField(ByteValue.ToString()) );
		ByteValue = (int)GUILayout.HorizontalSlider(ByteValue, 0, 254);
		
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();	
		
		ShowData = GUILayout.Toggle(ShowData, "Show Data");
		
		GUILayout.Label(string.Format("Corruption: {0:P}", ((mCurrentGlitchiness * mCurrentIterations) / textureData.bytes.Length)), GUI.skin.button);
		
		GUILayout.EndHorizontal();	
		
		GUILayout.EndVertical();
		
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
