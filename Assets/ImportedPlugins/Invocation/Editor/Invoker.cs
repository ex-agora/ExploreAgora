using UnityEngine;
using UnityEditor;

using System.Collections.Generic;
using System.Reflection;

public class Invoker : EditorWindow 
{
	//-------------------------------------------------------------------------------
	private enum eTypeMangle 
	{
        Boolean = 1,
		Integer,
		Single,
		Text,
		Vector2,
		Vector3,
		Vector4,
		Color,
		Curve,
		//Add Gradients
	}

	//-------------------------------------------------------------------------------
	private static Dictionary<System.Type, eTypeMangle> m_TypeMangler = new Dictionary<System.Type, eTypeMangle>
	{
		{ typeof(bool), eTypeMangle.Boolean },

		{ typeof(short), eTypeMangle.Integer },
		{ typeof(int), eTypeMangle.Integer },
		{ typeof(long), eTypeMangle.Integer },
		
		{ typeof(float), eTypeMangle.Single },
		{ typeof(double), eTypeMangle.Single },
		
		{ typeof(string), eTypeMangle.Text },
		
		{ typeof(Vector2), eTypeMangle.Vector2 },
		{ typeof(Vector3), eTypeMangle.Vector3 },
		{ typeof(Vector4), eTypeMangle.Vector4 },
		{ typeof(Color), eTypeMangle.Color },
		{ typeof(AnimationCurve), eTypeMangle.Curve },
	};

    //-------------------------------------------------------------------------------
    public class ComponentParameter
    {
        public string m_Name = "Unknown Parameter";

        public System.Type m_Type = null;

        public object m_ParameterInstance = null;

        public ComponentParameter(string lName, System.Type lType)
        {
            m_Name = lName;
            m_Type = lType;

            if (m_Type.IsValueType)
            {
                m_ParameterInstance = System.Activator.CreateInstance(m_Type);
            }
        }
    }

    //-------------------------------------------------------------------------------
    public class ComponentMethod
    {
        public ComponentType m_Parent = null;

        public bool m_Show = false;

        public string m_MethodName = "Unknown Method";

        public MethodInfo m_MethodInfo = null;

        public bool m_Public = false;
        public System.Type m_ReturnType = null;

        public object m_ReturnInstance = null;

        public ComponentParameter[] m_ParameterArray = null;
    }

    //-------------------------------------------------------------------------------
    public class ComponentType
    {
        public string m_TypeName = "Unknown Type";

        public bool m_Show = false;

        public System.Type m_Type = null;

        public Component[] m_Instances = null;

        public ComponentMethod[] m_MethodArray = null;

        public string m_SearchPhrase = "";
    }
	
	//-------------------------------------------------------------------------------
    private GameObject[] m_Targets = new GameObject[] { };
    private List<ComponentType> m_ComponentList = new List<ComponentType>(); 
    private Vector2 m_ScrollPosition = Vector2.zero;

    //-------------------------------------------------------------------------------
    public static Invoker Singleton { get; private set; }

	//-------------------------------------------------------------------------------
    [MenuItem("Window/Invoker")]
    public static void Init()
	{
        Singleton = (Invoker)EditorWindow.GetWindow(typeof(Invoker));
    }

    //-------------------------------------------------------------------------------
    private void OnEnable()
    {
        OnSelectionChange();
    }
    
    //-------------------------------------------------------------------------------
    private void OnFocus()
    {
        OnSelectionChange();
    }

	//-------------------------------------------------------------------------------
	private void OnGUI()
	{
        if (m_ComponentList != null)
		{
			m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition);
            foreach (ComponentType lComponent in m_ComponentList)
			{
                EditorGUI.indentLevel = 0;
                lComponent.m_Show = EditorGUILayout.InspectorTitlebar(lComponent.m_Show, lComponent.m_Instances);
                if (lComponent.m_Show)
				{
                    EditorGUI.indentLevel = 1;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Search");
                    lComponent.m_SearchPhrase = EditorGUILayout.TextField(lComponent.m_SearchPhrase);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();

                    foreach (ComponentMethod lMethod in lComponent.m_MethodArray)
					{
                        if (lComponent.m_SearchPhrase.Equals("")
                            || lMethod.m_MethodName.ToLower().Contains(lComponent.m_SearchPhrase.ToLower()))
                        {
                            MethodGUI(lMethod);
                        }
					}
				}
			}
			EditorGUILayout.EndScrollView();
		}
		//EditorGUILayout.LabelField("Output");
        //Will eventually output information...
	}
	
	//-------------------------------------------------------------------------------
	private void MethodGUI(ComponentMethod lMethod)
	{
        EditorGUI.indentLevel = 1;
        EditorGUILayout.BeginHorizontal();
        lMethod.m_Show = EditorGUILayout.Foldout(lMethod.m_Show, lMethod.m_MethodName);
        if (GUILayout.Button("Execute"))
        {
            object[] lParameters = new object[lMethod.m_ParameterArray.Length];
            for (int lCount = 0; lCount < lMethod.m_ParameterArray.Length; ++lCount)
            {
                lParameters[lCount] = lMethod.m_ParameterArray[lCount].m_ParameterInstance;
            }
            try
            {
                for (int lCount = 0; lCount < lMethod.m_Parent.m_Instances.Length; ++lCount)
                {
                    object lObject = lMethod.m_MethodInfo.Invoke(lMethod.m_Parent.m_Instances[lCount], lParameters);
                    if (lObject != null)
                    {
                        Debug.Log("Invoker: " + lMethod.m_MethodName + " returned " + lObject.ToString());
                    }
                }
            }
            catch
            {
				//Debug.Log("Invoker: " + lMethod.m_MethodName + " returned " + lObject.ToString());
            }
        }
        EditorGUILayout.EndHorizontal();

        if (lMethod.m_Show)
        {
            EditorGUILayout.Space();
            if (lMethod.m_ParameterArray.Length > 0)
            {
                foreach (ComponentParameter lParameter in lMethod.m_ParameterArray)
                {
                    ParameterGUI(lParameter);
                }
            }
            else
            {
                EditorGUI.indentLevel = 2;
                EditorGUILayout.LabelField("No Parameters");
            }
        }
        EditorGUILayout.Space();
	}
	
	//-------------------------------------------------------------------------------
    private void ParameterGUI(ComponentParameter lParameter)
    {
        EditorGUI.indentLevel = 2;
        EditorGUILayout.BeginHorizontal();
        if (m_TypeMangler.ContainsKey(lParameter.m_Type))
		{
            switch (m_TypeMangler[lParameter.m_Type])
            {
                case eTypeMangle.Boolean:
                {
                    lParameter.m_ParameterInstance = EditorGUILayout.Toggle(lParameter.m_Name, (bool)lParameter.m_ParameterInstance);
                    break;
                }
                case eTypeMangle.Integer:
                {
                    lParameter.m_ParameterInstance = EditorGUILayout.IntField(lParameter.m_Name, (int)lParameter.m_ParameterInstance);
                    break;
                }
                case eTypeMangle.Single:
                {
                    lParameter.m_ParameterInstance = EditorGUILayout.FloatField(lParameter.m_Name, (float)lParameter.m_ParameterInstance);
                    break;
                }
                case eTypeMangle.Text:
                {
                    lParameter.m_ParameterInstance = EditorGUILayout.TextField(lParameter.m_Name, (string)lParameter.m_ParameterInstance);
                    break;
                }
                case eTypeMangle.Vector2: 
                {
                    lParameter.m_ParameterInstance = EditorGUILayout.Vector2Field(lParameter.m_Name, (Vector2)lParameter.m_ParameterInstance);
                    break;
                }
                case eTypeMangle.Vector3:
                {
                    lParameter.m_ParameterInstance = EditorGUILayout.Vector3Field(lParameter.m_Name, (Vector3)lParameter.m_ParameterInstance);
                    break;
                }
                case eTypeMangle.Vector4:
                {
                    lParameter.m_ParameterInstance = EditorGUILayout.Vector4Field(lParameter.m_Name, (Vector4)lParameter.m_ParameterInstance);
                    break;
                }
                case eTypeMangle.Color:
                {
                    lParameter.m_ParameterInstance = EditorGUILayout.ColorField(lParameter.m_Name, (Color)lParameter.m_ParameterInstance);
                    break;
                }
                case eTypeMangle.Curve:
                {
                    lParameter.m_ParameterInstance = EditorGUILayout.CurveField(lParameter.m_Name, (AnimationCurve)lParameter.m_ParameterInstance);
                    break;
                }
            }
		}
		else
		{
            EditorGUILayout.PrefixLabel(lParameter.m_Name);
            if (lParameter.m_Type.IsEnum)
            {
                lParameter.m_ParameterInstance = EditorGUILayout.EnumPopup((System.Enum)lParameter.m_ParameterInstance);
            }
            else if (lParameter.m_Type.IsSubclassOf(typeof(MonoBehaviour)))
            {
                lParameter.m_ParameterInstance = EditorGUILayout.ObjectField((MonoBehaviour)lParameter.m_ParameterInstance, lParameter.m_Type, true);
            }
            else if (lParameter.m_Type.IsSubclassOf(typeof(Component)))
            {
                lParameter.m_ParameterInstance = EditorGUILayout.ObjectField((Component)lParameter.m_ParameterInstance, lParameter.m_Type, true);
            }
            else if (lParameter.m_Type.BaseType == typeof(Object))
            {
                lParameter.m_ParameterInstance = EditorGUILayout.ObjectField((Object)lParameter.m_ParameterInstance, lParameter.m_Type, true);
            }
            else
            {
                EditorGUILayout.LabelField("Type Not Supported.");
            }
		}
        EditorGUILayout.EndHorizontal();
	}
	
	//-------------------------------------------------------------------------------
	private void OnSelectionChange()
	{
        Object[] lTargets = Selection.GetFiltered(typeof(GameObject), SelectionMode.Unfiltered);

        if (lTargets.Length > 0
            && m_Targets.Length == lTargets.Length
            && m_Targets[0] == lTargets[0])
        {
            return;
        }

        m_Targets = new GameObject[lTargets.Length];
        for (int lCount = 0; lCount < lTargets.Length; ++lCount)
        {
            m_Targets[lCount] = lTargets[lCount] as GameObject;
        }

        m_ComponentList.Clear();
        m_ScrollPosition = Vector2.zero;

        Dictionary<System.Type, int> lCommonScripts = new Dictionary<System.Type, int>();
        foreach (GameObject lGo in m_Targets)
        {
            MonoBehaviour[] lComponentArray = lGo.GetComponents<MonoBehaviour>();
            System.Type lType;
            foreach (MonoBehaviour lMonoBehaviour in lComponentArray)
            {
                lType = lMonoBehaviour.GetType();
                if (lCommonScripts.ContainsKey(lType))
                {
                    ++lCommonScripts[lType];
                }
                else
                {
                    lCommonScripts.Add(lType, 1);
                }
            }
        }

        int lSubSlassCount = 0;
        foreach (KeyValuePair<System.Type, int> lTypeCount in lCommonScripts)
        {
            lSubSlassCount = 0;
            if (lTypeCount.Value < m_Targets.Length)
            {
                for (int lCount = 0; lCount < m_Targets.Length; ++lCount)
                {
                    if (m_Targets[lCount].GetComponent(lTypeCount.Key) != null)
                    {
                        ++lSubSlassCount;
                    }
                }
            }

            if (lTypeCount.Value >= m_Targets.Length
                || lSubSlassCount >= m_Targets.Length)
            {
                System.Type lType = lTypeCount.Key;

                List<MethodInfo> lMethodList = new List<MethodInfo>();

                while (lType.IsSubclassOf(typeof(MonoBehaviour)))
                {
                    lMethodList.AddRange(lType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));
                    lMethodList.AddRange(lType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
                    lType = lType.BaseType;
                }
				
				lType = lTypeCount.Key;
                Component[] lComponents = new Component[m_Targets.Length];
                for (int lCount = 0; lCount < lComponents.Length; ++lCount)
                {
                    lComponents[lCount] = m_Targets[lCount].GetComponent(lType);
                }

                m_ComponentList.Add(new ComponentType
                {
                    m_TypeName = lType.Name,
                    m_Type = lType,
                    m_Instances = lComponents,
                });

                ComponentType lNewComponentType = m_ComponentList[m_ComponentList.Count - 1];

                lNewComponentType.m_MethodArray = new ComponentMethod[lMethodList.Count];
                for (int lMethodCount = 0; lMethodCount < lMethodList.Count; ++lMethodCount)
                {
                    lNewComponentType.m_MethodArray[lMethodCount] = new ComponentMethod
                    {
                        m_MethodName = lMethodList[lMethodCount].Name,
                        m_MethodInfo = lMethodList[lMethodCount],
                        m_Parent = m_ComponentList[m_ComponentList.Count - 1],
                    };

                    ParameterInfo[] lParameterArray = lMethodList[lMethodCount].GetParameters();
                    lNewComponentType.m_MethodArray[lMethodCount].m_ParameterArray = new ComponentParameter[lParameterArray.Length];
                    for (int lParameterCount = 0; lParameterCount < lParameterArray.Length; ++lParameterCount)
                    {
                        lNewComponentType.m_MethodArray[lMethodCount].m_ParameterArray[lParameterCount] = new ComponentParameter
                        (
                            lParameterArray[lParameterCount].Name,
                            lParameterArray[lParameterCount].ParameterType
                        );
                    }
                }  
            }
        }
	
		Repaint();
	}
}
