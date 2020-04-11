using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System;

namespace ArjunaneLibrary
{
    public class Arjunane
	{
		ServerExecute server;
		/*
			HOW TO USE:
			first = Arjunane ar = new Arjunane();
			|| DOCUMENTATION FILE : file:///E:/Augmented%20Reality/Library%20Arjunane/documentation.html
			===============================================================================
											ELEMENT GAMEOBJECT
			===============================================================================
			=== Get element gameobject name ===
			[FIRST-CHAIN || need string]
			==> ar.GetElem("name_gameobject");

			=== Get Index of Elements ===
			[LAST-CHAIN || return int]
			ar.index();

			=== Get Index of Elements ===
			[LAST-CHAIN || need int]
			==> ar.IndexOf(index);
			
			=== Set Elements to Array ===
			[LAST-CHAIN || return GameObject[] ]
			==> ar.SetEls();

			=== Set Elements to GameObject ===
			[LAST-CHAIN || return GameObject ]
			==> ar.SetEl();

			=== Get GameObject to RawImage ===
			[LAST-CHAIN || return RawImage ]
			==> ar.RawImage();

			=== Get GameObject to Text ===
			[LAST-CHAIN || return Text ]
			==> ar.Text();

			=== Set GameObject Material Color ===
			[LAST-CHAIN || need <GameObject, Color>]
			==> ar.setMaterial(GameObject, Color);

			[LAST-CHAIN] ar.click(() => {
				=== ACTION ===
			});
			
			===============================================================================
											WEB SERVER
			===============================================================================
			
			=== Get URL / LINK ===
			[FIRST-CHAIN || need string]
			==> ar.GetURL("your_url");

			|| === Set Raw Image via URL ===
			|| [LAST-CHAIN || need GameObject]
			|| ==> ar.LoadImage( GameObject );

			=== FORM POST ===
			|| [CHAIN || need <string, string> ]
			|| ==> ar.AddField( "field", "value" ); // one per one
			|| OR
			|| [CHAIN || need Dictionary<string, string>]
			|| ==> ar.AddFields(dict); // all field in Dictionary
			|| 
			|| === FORM EXECUTE ===
			|| [CHAIN ]
			|| ==> ar.Form();
			|| 

			=== POST SERVER ===
			|| === SET SERVER WEBSITE ===
			|| [CHAIN]
			|| ==> ar.SendWeb();

			=== LAST CHAIN SERVER ===
			|| === RESULT FROM FORM EXECUTE ===
			|| [LAST-CHAIN || return boolean]
			|| ==> ar.GetResultServer();
			|| 
			|| === SHOW ERROR FROM FORM EXECUTE ===
			|| [LAST-CHAIN || return string]
			|| ==> ar.ShowError();
			|| 
			|| === SHOW TEXT FROM FORM EXECUTE ===
			|| [LAST-CHAIN || return string]
			|| ==> ar.SetTextHTTP();
		*/

		// Use this for initialization
		private  GameObject[] ini;
		private string isServerDone = "";
		private  GameObject ini_index;
		private string _elem, _url, _errorHTTP, _textHTTP;
		private int _index = 0;
		public string version = "1.1.0 [Arjunane]";
		private bool _resServer;
		private Dictionary<string, string> _field = new Dictionary<string,string>();
		/*
			GAMEOBJECT
		*/
		//get name element GameObject
		public Arjunane GetElem(string elem = null) 
		{
			bool isExist = false;
			string[] split_comma = elem.Split(',');
			List<GameObject> list = new List<GameObject>();
			// split berdasarkan koma
			for(int c = 0; c < split_comma.Length; c++)
			{
				string name_split = split_comma[c].Trim();
				GameObject[] getTag = Resources.FindObjectsOfTypeAll<GameObject>();
				for(int i = 0; i < getTag.Length; i++)
				{
					GameObject _i = getTag[i];
					string     name = _i.transform.name.ToString();
					if(name_split == name)
					{
						isExist = true;
						list.Add(_i);
					}
				}
			}
			if(!isExist)
			{
				throwError();
			}

			ini = NormalizeSortGameObject(list.ToArray());
			_elem = elem;

			return this;
		}
		private GameObject[] NormalizeSortGameObject( GameObject[] go )
		{
			List< GetList > newList = new List< GetList >();
			for(int i = 0; i < go.Length; i++)
			{
				GameObject  ini_go 	= go[i];
				int			sibling = ini_go.transform.GetSiblingIndex();
				newList.Add(new GetList{gameObject = ini_go, sibling = sibling});
			} 

			// normalisasi pengurutan berdasarkan urutan pada hierarchy
			newList.Sort(delegate(GetList x, GetList y) {
				return x.sibling.CompareTo(y.sibling);
			});

			List< GameObject > setList = new List< GameObject >();

			for(int i = 0; i < newList.Count; i++)
			{
				var ini_list = newList[i];
				GameObject  ini_go 	= ini_list.gameObject;
				setList.Add(ini_go);
			}
			GameObject[] arr = setList.ToArray();
			return arr;
		}
		public void Clone(GameObject child, GameObject parent, int how_many = 1, bool world_position = false)
		{
			for(int i = 0; i < how_many; i++)
			{
				GameObject instantiate 	= GameObject.Instantiate(child) as GameObject;
				instantiate.name = instantiate.name.Replace("(Clone)", "");
				instantiate.transform.SetParent(parent.transform, world_position);
			}
		}
		public Arjunane IndexOf(int index)
		{
			_index = index;
			ini_index = ini[index];
			return this;
		}
		public GameObject[] GameObjects()
		{
			return ini;
		}
		private GameObject callBack_ini (int index = -1)
		{
			GameObject go;
			if(ini_index == null)
			{
				if(index == -1) go = ini[0];
				else			go = ini[index];
			}
			else 
			{
				go = ini_index;
			}
			return go;
		}
		public void ForEach(UnityAction<GameObject, int> callback)
		{
			for(int i = 0; i < ini.Length; i++)
			{
				GameObject get = ini[i];
				string name = get.transform.name;
				callback(get, i);
			}
		}
		public int Index()
		{
			return _index;
		}
		public GameObject SetEl()
		{
			GameObject _ini = callBack_ini();
			return _ini;
		}
		public Animator Animator()
		{
			GameObject _ini = callBack_ini();
			Animator tex = _ini.GetComponent<Animator>();
			return tex;
		}
		public RawImage RawImage()
		{
			GameObject _ini = callBack_ini();
			RawImage ri = _ini.GetComponent<RawImage>();
			return ri;
		}
		public InputField InputField()
		{
			GameObject _ini = callBack_ini();
			InputField tex = _ini.GetComponent<InputField>();
			return tex;
		}
		public Texture Texture()
		{
			GameObject _ini = callBack_ini();
			Texture tex = _ini.GetComponent<Renderer>().material.GetTexture("_MainTex");
			return tex;
		}
		public Text Text()
		{
			GameObject go = callBack_ini();
			Text text = go.GetComponent<Text>();
			return text;
		}
		public Text GetText(GameObject ini)
		{
			Text text = ini.GetComponent<Text>();
			return text;
		}
		public Animator GetAnimator(GameObject ini)
		{
			Animator anim = ini.GetComponent<Animator>();
			return anim;
		}
		public InputField GetInputField(GameObject ini)
		{
			InputField anim = ini.GetComponent<InputField>();
			return anim;
		}
		public RawImage GetRawImage(GameObject ini)
		{
			RawImage ri = ini.GetComponent<RawImage>();
			return ri;
		}
		public Texture GetTexture(GameObject ini)
		{
			Texture tex = ini.GetComponent<Renderer>().material.GetTexture("_MainTex");
			return tex;
		}
		public Animator ToAnimator(int index)
		{
			GameObject go = callBack_ini(index);
			Animator anim = go.GetComponent<Animator>();
			return anim;
		}
		public InputField ToInputField(int index)
		{
			GameObject go = callBack_ini(index);
			InputField anim = go.GetComponent<InputField>();
			return anim;
		}
		public RawImage ToRawImage(int index)
		{
			GameObject go 	= callBack_ini(index);
			RawImage ri 	= go.GetComponent<RawImage>();
			return ri;
		}
		public Texture ToTexture(int index)
		{
			GameObject go = callBack_ini(index);
			Texture tex = go.GetComponent<Renderer>().material.GetTexture("_MainTex");
			return tex;
		}
		public Text ToText(int index)
		{
			GameObject go = callBack_ini(index);
			Text text = go.GetComponent<Text>();
			return text;
		}
		// Get Component GameObject to RawImage
		public RawImage[] RawImages()
		{
			List<RawImage> list = new List<RawImage>();
			for(int i = 0; i < ini.Length; i++)
			{
				RawImage text = ini[i].GetComponent<RawImage>();
				list.Add(text);
			}
			return list.ToArray();
		}
		public InputField[] InputFields()
		{
			List<InputField> list = new List<InputField>();
			for(int i = 0; i < ini.Length; i++)
			{
				InputField text = ini[i].GetComponent<InputField>();
				list.Add(text);
			}
			return list.ToArray();
		}
		public Texture[] Textures()
		{
			List<Texture> list = new List<Texture>();
			for(int i = 0; i < ini.Length; i++)
			{
				Texture text = ini[i].GetComponent<Texture>();
				list.Add(text);
			}
			return list.ToArray();
		}
		public Text[] Texts()
		{
			List<Text> list = new List<Text>();
			for(int i = 0; i < ini.Length; i++)
			{
				Text text = ini[i].GetComponent<Text>();
				list.Add(text);
			}
			return list.ToArray();
		}
		public void SetMaterial(GameObject go, string hex)
		{
			Color col = new Color();
			if(ColorUtility.TryParseHtmlString(hex, out col)) 
			{
				go.GetComponent<Renderer>().material.color = col;
			}
		} 
		public void SetTexture(GameObject go, Texture tex)
		{
			go.GetComponent<Renderer>().material.SetTexture("_MainTex", tex);
		} 
		private void throwError(int name = 0)
		{
			if		(name == 0) 	Debug.LogError("Ops, Element with name " + _elem + " doesn't Exist.");
			else if	(name == 1)    	Debug.LogError("Please add Function for action click.");
			else if	(name == 2)    	Debug.LogError("GameObject is not Button.");
		}
		public void Click(UnityAction method)
		{
			// if element Untagged or something is exist
			if(ini.Length != 0)
			{
				// check if action method are not null
				if(method != null)
				{
					if(ini_index != null)
					{
						ini_index.GetComponent<Button>().onClick.AddListener( () => {
							method();
						});
					}
					else
					{
						for(int i = 0; i < ini.Length; i++)
						{
							int index = i;
							GameObject _ini = ini[index];
							Button btn		= _ini.GetComponent<Button>();

							if(btn != null)
							{
								btn.onClick.AddListener(() => {
									_index  = index;
									method();
								});
							}
						}	
					}
				}
				else
				{
					throwError(1);
				}
			}
			else
			{
				throwError();
			}
		}

		/*
			SERVER WEBSITE
		*/
		public Arjunane GetURL(string url = null)
		{
			_field.Clear();
			_url = url;
			return this;
		}
		public Arjunane AddField(string field, string value)
		{
			_field.Add(field, value);
			return this;
		}
		public Arjunane AddFields(Dictionary<string,string> _dict)
		{
			_field = _dict;
			return this;
		}
		public IEnumerator SendForm(UnityAction<string, bool, string> callback, int timeout = 60)
		{
			// set to the server
			WWWForm form = new WWWForm();
			foreach(var item in _field)
			{
				form.AddField(item.Key, item.Value);
			}
			//Debug.Log(System.Text.Encoding.UTF8.GetString(form.data));
			UnityWebRequest proses = UnityWebRequest.Post(_url, form);
			proses.timeout = timeout;
			yield return proses.SendWebRequest();
			if(proses.isNetworkError || proses.isHttpError)
			{
				_resServer = false;
				_errorHTTP = proses.error;
				Debug.LogError(proses.error);
			}
			else
			{
				_resServer = true;
				_textHTTP = proses.downloadHandler.text;
			}
			callback(_textHTTP, _resServer, _errorHTTP);
		}
		private IEnumerator _form()
		{
			WWWForm form = new WWWForm();
			foreach(var item in _field)
			{
				form.AddField(item.Key, item.Value);
			}

			var proses = UnityWebRequest.Post(_url, form);

			yield return proses.SendWebRequest();
			isServerDone = "done";
			if(proses.isNetworkError || proses.isHttpError)
			{
				_resServer = false;
				_errorHTTP = proses.error;
				Debug.LogError(proses.error);
			}
			else
			{
				_resServer = true;
				_textHTTP = proses.downloadHandler.text;
			}
		}

		public IEnumerator SendWeb(UnityAction<string, bool, string> callback)
		{
			WWW www = new WWW (_url);
			yield return www;
			if(www.error == null)
			{
				_resServer = true;
				_textHTTP  = www.text;
			}
			else
			{
				_resServer = false;
				_errorHTTP = www.error;
				Debug.LogError(www.error);
			}
			callback(_textHTTP, _resServer, _errorHTTP);
		}
		private IEnumerator _web()
		{
			WWW www = new WWW (_url);
			yield return www;
			isServerDone = "done";
			if(www.error == null)
			{
				_resServer = true;
				_textHTTP  = www.text;
			}
			else
			{
				_resServer = false;
				_errorHTTP = www.error;
				Debug.LogError(www.error);
			}
		}

		public string SetTextHTTP()
		{
			return _textHTTP;
		}
		public string ShowError()
		{
			return _errorHTTP; 
		}

		public bool GetResultServer()
		{
			while(isServerDone != "")
			{
				Debug.Log(isServerDone);
				return _resServer;
			}
			return true;
		}
		public void LoadImage(GameObject raw)
		{
			server.Execute(_LoadImage(raw));
		}
		private IEnumerator _LoadImage(GameObject raw)
		{
			WWW w3 = new WWW(_url);
			RawImage ri = raw.GetComponent<RawImage>();
			yield return w3;
			isServerDone = "done";
			if(w3.error == null)
			{
				ri.texture = w3.texture;
			}
		}

		/*
			OTHER FUNCTION
		*/
		public string SetDate(string date, bool monthName = true, bool wib = false, string split = "-")
		{
			// split space date if time is exist
			string[] _date 		= date.Split(" ".ToCharArray() );
			string[] _monthName = new string[12];

			string _setDate     = _date[0];

			_monthName[0]		= "Januari";
			_monthName[1]		= "Februari";
			_monthName[2]		= "Maret";
			_monthName[3]		= "April";
			_monthName[4]		= "Mei";
			_monthName[5]		= "Juni";
			_monthName[6]		= "July";
			_monthName[7]		= "Agustus";
			_monthName[8]		= "September";
			_monthName[9]		= "Oktober";
			_monthName[10]		= "November";
			_monthName[11]		= "Desember";

			string[] _splitDate   = _date[0].Split( "-".ToCharArray() );

			if(monthName == true)
			{
				_setDate = _splitDate[0] + split + _monthName[Int16.Parse(_splitDate[1])] + split + _splitDate[2];
			}
			
			string _wib = (wib == true) ? " WIB" : "";
			string _dateString = _setDate + " " + _date[1] + _wib;
			return _dateString;
		}
	}
}
class GetList 
{
	public GameObject gameObject {get;set;}
    public int sibling {get;set;}
}
class ServerExecute : MonoBehaviour
{
	private class CoroutineHolder : MonoBehaviour { }
 
    //lazy singleton pattern. Note that I don't set it to dontdestroyonload - you usually want corotuines to stop when you load a new scene.
    private static CoroutineHolder _runner;
    private static CoroutineHolder runner {
        get {
            if (_runner == null) {
                _runner = new GameObject("Static Corotuine Runner").AddComponent<CoroutineHolder>();
            }
            return _runner;
        }
    }
 
    public  void Execute(IEnumerator corotuine) {
        runner.StartCoroutine(corotuine);
		DestroyImmediate(GameObject.Find("Static Corotuine Runner"));
    }
	public void Clone(GameObject child, GameObject parent)
	{
		Debug.Log(child + " | " + parent);
		GameObject instantiate 	= Instantiate(child);
		instantiate.name = instantiate.name.Replace("(Clone)", "");
		instantiate.transform.SetParent(parent.transform);
	}
}