using UnityEngine;
using System.Collections.Generic;

// http://sourceforge.net/p/urgnetwork/wiki/top_jp/
// https://www.hokuyo-aut.co.jp/02sensor/07scanner/download/pdf/URG_SCIP20.pdf
public class URGManager : MonoBehaviour 
{
	class DetectObject
	{
		public List<long> distList;
		public List<int> idList;

		public long startDist;
		public DetectObject()
		{
			distList = new List<long>();
			idList = new List<int>();
		}
	}

    [Space]
    [Header("Point Prefab")]
    [SerializeField]
    private GameObject PointPrefab;

    [Space]
    [Header("Point Parent")]
    [SerializeField]
    private Transform PointP;

    [Space]
    [Header("IP Address")]
    public string strIPAddress = "192.168.0.10";

    [Space]
    [Header("Port Number")]
    public int iPortNumber = 10940;

    [Space]
    [Header("Detect Objects")]
    private List<DetectObject> DetectObjects;

    [Space]
    [Header("Directions")]
    private Vector3[] Directions;

    [Space]
    [Header("Cache Boolean")]
    private bool bCached = false;

    [Space]
    [Header("URG")]
    UrgDeviceEthernet URG;

    [Space]
    [Header("Scale")]
    public float fScale = 0.1f;

    [Space]
    [Header("X Scale Multi")]
    public float fXScaleMulti = 1.0f;

    [Space]
    [Header("Limit")]
    public float fLimit = 300.0f;//mm

    [Space]
    [Header("Noise Limit")]
    public int iNoiseLimit = 5;

    [Space]
    [Header("X Minimize Position")]
    public float fXMinPos = 0;

    [Space]
    [Header("Y Minimize Position")]
    public float fYMinPos = 0;

    [Space]
    [Header("Distance Color")]
    [SerializeField]
    private Color cDistanceColor = Color.white;

    //[Space]
    //[Header("Detect Color")]
    //[SerializeField]
    //private Color cDetectColor = Color.white;

    //[Space]
    //[Header("Strength Color")]
    //[SerializeField]
    //private Color cStrengthColor = Color.white;

    //[Space]
    //[Header("Group Colors")]
    //public Color[] GroupColors;

    [Space]
    [Header("Distances")]
    private List<long> Distances;

    [Space]
    [Header("Strengths")]
    private List<long> Strengths;


    [Space]
    [Header("Debug Draw Boolean")]
    [SerializeField]
    //private Rect AreaRect;

    //[Space]
    //[Header("Debug Draw Boolean")]
    //[SerializeField]
    private bool bDebugDraw = false;

    [Space]
    [Header("Draw Count")]
    private int iDrawCount;

    [Space]
    [Header("URG ID")]
    [SerializeField]
    public int URGID;

    private void Awake()
    {
        // Check if there is already an instance of this object
        //if (FindObjectsOfType<URGManager>().Length > 1)
        //{
        //    // If there is, destroy this object
        //    //Debug.Log("Destroy");
        //    Destroy(this.gameObject);
        //}
        //else
        //{
        //    // Otherwise, make this object persistent
        //    //Debug.Log("DontDestroyOnLoad");
        //    DontDestroyOnLoad(this.gameObject);
        //}
    }

    // Use this for initialization
    void Start () 
	{
        Distances = new List<long>();
        Strengths = new List<long>();

        URG = this.gameObject.AddComponent<UrgDeviceEthernet>();
        URG.StartTCP(strIPAddress, iPortNumber);

        URG.Write(SCIP_library.SCIP_Writer.ME(0, 1080, 1, 1, 0));

        if (PointP.childCount != 0)
        {
           
        }
        else if (PointP.childCount == 0)
        {
            
        }
    }
	
	// Update is called once per frame
	void Update () 
	{
        if (PointP.childCount != 0)
        {
            
        }
        else if (PointP.childCount == 0)
        {
           
        }

        if (gd_loop)
		{
            URG.Write(SCIP_library.SCIP_Writer.GD(0, 1080));
		}

        // center offset rect
        //Rect detectAreaRect = AreaRect;
        //detectAreaRect.x *= scale;
        //detectAreaRect.y *= scale;
        //detectAreaRect.width *= scale;
        //detectAreaRect.height *= scale;

        //detectAreaRect.x = -detectAreaRect.width / 2;
        //

        float d = Mathf.PI * 2 / 1440;
		float offset = d * 540;

		// cache directions
		if(URG.distances.Count > 0 )
		{
			if(!bCached)
			{
                Directions = new Vector3[URG.distances.Count];
				for(int i = 0; i < Directions.Length; i++)
				{
					float a = d * i + offset;
                    Directions[i] = new Vector3(-Mathf.Cos(a), -Mathf.Sin(a), 0);
				}
                bCached = true;
			}
		}

		// strengths
		try
		{
			if(URG.strengths.Count > 0)
			{
                Strengths.Clear();
                Strengths.AddRange(URG.strengths);
			}
		}
		catch
		{
		}
		// distances
		try
		{
			if(URG.distances.Count > 0)
			{
                Distances.Clear();
                Distances.AddRange(URG.distances);
			}
		}
		catch
		{
		}
//		List<long> distances = urg.distances;

		if(bDebugDraw)
		{
			// strengths
			for(int i = 0; i < Strengths.Count; i++)
			{
				//float a = d * i + offset;
				//Vector3 dir = new Vector3(-Mathf.Cos(a), -Mathf.Sin(a), 0);
				Vector3 dir = Directions[i];
				long dist = Strengths[i];
                //Debug.DrawRay(Vector3.zero, Mathf.Abs( dist ) * dir * scale, cStrengthColor);
            }

            // distances
            //			float colorD = 1.0f / 1440;
            for (int i = 0; i < Distances.Count; i++)
			{
				//float a = d * i + offset;
				//Vector3 dir = new Vector3(-Mathf.Cos(a), -Mathf.Sin(a), 0);
				Vector3 dir = Directions[i];
				long dist = Distances[i];
                //color = (dist < limit && dir.y > 0) ? cDetectColor : new Color(colorD * i, 0,0,1.0f);
                //Color color = (dist < limit && dir.y > 0) ? cDetectColor : distanceColor;
                //				Debug.DrawRay(Vector3.zero, dist * dir * scale, color);
                //Debug.DrawRay(Vector3.zero, dist * -dir * scale, distanceColor);
                Vector3 localDir = new Vector3(-dir.x * fXScaleMulti, -dir.y, -dir.z); // 感測器「局部」方向
                Vector3 worldDir = transform.TransformDirection(localDir);             // 轉到世界方向
                Debug.DrawRay(transform.position, (float)dist * fScale * worldDir, cDistanceColor);
                //Debug.DrawRay(this.transform.position, dist * new Vector3(-dir.x * fScale * fXScaleMulti, -dir.y * fScale, -dir.z * fScale), cDistanceColor);
            }
		}

        //-----------------
        //  group
        DetectObjects = new List<DetectObject>();
		//
		//------
//		bool endGroup = true;
//		for(int i = 0; i < distances.Count; i++){
//			int id = i;
//			long dist = distances[id];
//
//			float a = d * i + offset;
//			Vector3 dir = new Vector3(-Mathf.Cos(a), -Mathf.Sin(a), 0);
//
//			if(dist < limit && dir.y > 0){
//				DetectObject detect;
//				if(endGroup){
//					detect = new DetectObject();
//					detect.idList.Add(id);
//					detect.distList.Add(dist);
//
//					detect.startDist = dist;
//					detectObjects.Add(detect);
//					
//					endGroup = false;
//				}else{
//					detect = detectObjects[detectObjects.Count-1];
//					detect.idList.Add(id);
//					detect.distList.Add(dist);
//
//					if(dist > detect.startDist){
//						endGroup = true;
//					}
//				}
//			}else{
//				endGroup = true;
//			}
//		}

		//------
//		bool endGroup = true;
//		for(int i = 1; i < distances.Count-1; i++){
//			long dist = distances[i];
//			float delta = Mathf.Abs((float)(distances[i] - distances[i-1]));
//			float delta1 = Mathf.Abs((float)(distances[i+1] - distances[i]));
//			
//			float a = d * i + offset;
//			Vector3 dir = new Vector3(-Mathf.Cos(a), -Mathf.Sin(a), 0);
//			
//			if(dir.y > 0){
//				DetectObject detect;
//				if(endGroup){
//					if(dist < limit && delta > 50){
//						detect = new DetectObject();
//						detect.idList.Add(i);
//						detect.distList.Add(dist);
//						
//						detect.startDist = dist;
//						detectObjects.Add(detect);
//						
//						endGroup = false;
//					}
//				}else{
//					if(delta < 50){
//						detect = detectObjects[detectObjects.Count-1];
//						detect.idList.Add(i);
//						detect.distList.Add(dist);
//					}else{
//						endGroup = true;
//					}
//				}
//			}
//		}


		//------
		bool endGroup = true;
		float deltaLimit = 100; // 認識の閾値　連続したもののみを取得するため (mm)
		for(int i = 1; i < Distances.Count-1; i++)
		{
			//float a = d * i + offset;
			//Vector3 dir = new Vector3(-Mathf.Cos(a), -Mathf.Sin(a), 0);
			Vector3 dir = Directions[i];
			long dist = Distances[i];
			float delta = Mathf.Abs((float)(Distances[i] - Distances[i-1]));
			float delta1 = Mathf.Abs((float)(Distances[i+1] - Distances[i]));
				
			//if(dir.y > 0)
			//{
				DetectObject detect;
				if(endGroup)
				{
					Vector3 pt = dist * new Vector3(dir.x * fScale * fXScaleMulti, dir.y * fScale, dir.z * fScale);
					if(dist < fLimit && (delta < deltaLimit && delta1 < deltaLimit) && Mathf.Abs(pt.x) <= fXMinPos && Mathf.Abs(pt.y) <= fYMinPos)
					{
//					bool isArea = detectAreaRect.Contains(pt);
//					if(isArea && (delta < deltaLimit && delta1 < deltaLimit)){
						detect = new DetectObject();
						detect.idList.Add(i);
						detect.distList.Add(dist);
						
						detect.startDist = dist;
                        DetectObjects.Add(detect);
						
						endGroup = false;
					}
				}
				else
				{
					if(delta1 >= deltaLimit || delta >= deltaLimit)
					{
						endGroup = true;
					}
					else
					{
						detect = DetectObjects[DetectObjects.Count-1];
						detect.idList.Add(i);
						detect.distList.Add(dist);
					}
				}
			//}
		}

		//-----------------
		// draw 

		for (int i = DetectObjects.Count; i < PointP.childCount; i++)
		{
			Destroy(PointP.GetChild(i).gameObject);
		}

        iDrawCount = 0;
		for(int i = 0; i < DetectObjects.Count; i++)
		{
			DetectObject detect = DetectObjects[i];

			// noise
			if(detect.idList.Count < iNoiseLimit)
			{
				continue;
			}

			int offsetCount = detect.idList.Count / 3;
			int avgId = 0;
			for(int n = 0; n < detect.idList.Count; n++)
			{
				avgId += detect.idList[n];
			}
			avgId = avgId / (detect.idList.Count);

			long avgDist = 0;
			for(int n = offsetCount; n < detect.distList.Count - offsetCount; n++)
			{
				avgDist += detect.distList[n];
			}
			avgDist = avgDist / (detect.distList.Count - offsetCount * 2);

			//float a = d * avgId + offset;
			//Vector3 dir = new Vector3(-Mathf.Cos(a), -Mathf.Sin(a), 0);
			Vector3 dir = Directions[avgId];
			long dist = avgDist;


			//float a0 = d * detect.idList[offsetCount] + offset;
			//Vector3 dir0 = new Vector3(-Mathf.Cos(a0), -Mathf.Sin(a0), 0);
			int id0 = detect.idList[offsetCount];
			Vector3 dir0 = Directions[id0];
			long dist0 = detect.distList[offsetCount];

			//float a1 = d * detect.idList[detect.idList.Count-1 - offsetCount] + offset;
			//Vector3 dir1 = new Vector3(-Mathf.Cos(a1), -Mathf.Sin(a1), 0);
			int id1 = detect.idList[detect.idList.Count-1 - offsetCount];
			Vector3 dir1 = Directions[id1];
			long dist1 = detect.distList[detect.distList.Count-1 - offsetCount];

            //Color gColor;
            //if(iDrawCount < groupColors.Length)
            //{
            //	gColor = groupColors[iDrawCount];
            //}
            //else
            //{
            //	gColor = Color.green;
            //}

            for (int j = offsetCount; j < detect.idList.Count - offsetCount; j++)
			{
				//float _a = d * detect.idList[j] + offset;
				//Vector3 _dir = new Vector3(-Mathf.Cos(_a), -Mathf.Sin(_a), 0);
				int _id = detect.idList[j];
				Vector3 _dir = Directions[_id];
				long _dist = detect.distList[j];
				//Debug.DrawRay(Vector3.zero, _dist * -_dir * scale, gColor);
				//Debug.DrawRay(this.transform.position, _dist * -_dir * scale, gColor);
			}

            //Debug.DrawLine(dist0 * -dir0 * scale, dist1 * -dir1 * scale, gColor);
            //Debug.DrawLine(this.transform.position, dist1 * -dir1 * scale, gColor);
            //Debug.DrawRay(Vector3.zero, dist * -dir * scale, Color.green);
            Vector3 localDir = new Vector3(-dir.x * fXScaleMulti, -dir.y, -dir.z);
            Vector3 worldDir = transform.TransformDirection(localDir);
            Debug.DrawRay(transform.position, (float)dist * fScale * worldDir, Color.green);
            //Debug.DrawRay(this.transform.position, dist * new Vector3(-dir.x * fScale * fXScaleMulti, -dir.y * fScale, -dir.z * fScale), Color.green);

			if (PointP.childCount - 1 < i)
			{
                var point = Instantiate(PointPrefab, PointP);
				point.GetComponent<PointerControl>().URGID = URGID;
                point.transform.localPosition = LocalPoint(dist, dir);
                //point.transform.position = this.transform.position + (dist * new Vector3(-dir.x * fScale * fXScaleMulti, -dir.y * fScale, -dir.z * fScale));
            }
			else
			{
                PointP.GetChild(i).position = transform.TransformPoint(LocalPoint(dist, dir));
                //PointP.GetChild(i).position = this.transform.position + (dist * new Vector3(-dir.x * fScale * fXScaleMulti, -dir.y * fScale, -dir.z * fScale));
            }

            iDrawCount++;
		}

		//DrawRect(detectAreaRect, Color.green);
	}

    Vector3 LocalPoint(float distVal, Vector3 dirVec)
    {
        // 先組「局部」座標點（單位：公尺/Unity 單位）
        return (float)distVal * fScale * new Vector3(-dirVec.x * fXScaleMulti, -dirVec.y, -dirVec.z);
    }
    //void DrawRect(Rect rect, Color color)
    //{
    //	Vector3 p0 = new Vector3(rect.x, rect.y, 0);
    //	Vector3 p1 = new Vector3(rect.x + rect.width, rect.y, 0);
    //	Vector3 p2 = new Vector3(rect.x + rect.width, rect.y + rect.height, 0);
    //	Vector3 p3 = new Vector3(rect.x, rect.y + rect.height, 0);
    //	Debug.DrawLine(p0, p1, color);
    //	Debug.DrawLine(p1, p2, color);
    //	Debug.DrawLine(p2, p3, color);
    //	Debug.DrawLine(p3, p0, color);
    //}

    private bool gd_loop = false;

	// PP
//	MODL ... センサ型式情報
//	DMIN ... 最小計測可能距離 (mm)
//	DMAX ... 最大計測可能距離 (mm)
//	ARES ... 角度分解能(360度の分割数)
//	AMIN ... 最小計測可能方向値
//	AMAX ... 最大計測可能方向値
//	AFRT ... 正面方向値
//	SCAN ... 標準操作角速度

//	void OnGUI()
//	{
//		// https://sourceforge.net/p/urgnetwork/wiki/scip_jp/
//		if(GUILayout.Button("VV: (バージョン情報の取得)")){
//			urg.Write(SCIP_library.SCIP_Writer.VV());
//		}
////		if(GUILayout.Button("SCIP2")){
////			urg.Write(SCIP_library.SCIP_Writer.SCIP2());
////		}
//		if(GUILayout.Button("PP: (パラメータ情報の取得)")){
//			urg.Write(SCIP_library.SCIP_Writer.PP());
//		}
//		if(GUILayout.Button("MD: (計測＆送信要求)")){
//			urg.Write(SCIP_library.SCIP_Writer.MD(0, 1080, 1, 0, 0));
//		}
//		if(GUILayout.Button("ME: (計測＆距離データ・受光強度値送信要求)")){
//			urg.Write(SCIP_library.SCIP_Writer.ME(0, 1080, 1, 1, 0));
//		}
//		if(GUILayout.Button("BM: (レーザの発光)")){
//			urg.Write(SCIP_library.SCIP_Writer.BM());
//		}
//		if(GUILayout.Button("GD: (計測済み距離データ送信要求)")){
//			urg.Write(SCIP_library.SCIP_Writer.GD(0, 1080));
//		}
//		if(GUILayout.Button("GD_loop")){
//			gd_loop = !gd_loop;
//		}
//		if(GUILayout.Button("QUIT")){
//			urg.Write(SCIP_library.SCIP_Writer.QT());
//		}

//		GUILayout.Label("distances.Count: "+distances.Count + " / strengths.Count: "+strengths.Count);
//		GUILayout.Label("drawCount: "+drawCount + " / detectObjects: "+detectObjects.Count);
//	}
}