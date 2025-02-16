using UnityEngine ;
using UnityEngine.UI ;
using DG.Tweening ;
using UnityEngine.Events ;
using System.Collections.Generic ;

namespace EasyUI.PickerWheelUI
{

    public class PickerWheel : MonoBehaviour
    {

        private Pextension.MiniPool<WheelPiece> piecePools = new();
        private Pextension.MiniPool<WheelLine> linePools = new();
        [Header("References :")]
        [SerializeField] private WheelLine linePrefab;
        [SerializeField] private Transform linesParent;

        [Space]
        [SerializeField] private Transform PickerWheelTransform;
        [SerializeField] private Transform wheelCircle;
        [SerializeField] private WheelPiece wheelPiecePrefab;
        [SerializeField] private Transform wheelPiecesParent;

        //[Space]
        //[Header ("Sounds :")]
        //[SerializeField] private AudioSource audioSource ;
        //[SerializeField] private AudioClip tickAudioClip ;
        //[SerializeField] [Range (0f, 1f)] private float volume = .5f ;
        //[SerializeField] [Range (-3f, 3f)] private float pitch = 1f ;

        [Space]
        [Header("Picker wheel settings :")]
        [Range(1, 20)] public int spinDuration = 8;
        [SerializeField] [Range(.2f, 2f)] private float wheelSize = 1f;

        [Space]
        [Header("Picker wheel pieces :")]
        [SerializeField] private List<WheelDataConfigs<GiftType>> m_ListWheelPiecesData;
        [SerializeField] private List<WheelPiece> m_ListWheelPieces;
        [SerializeField] private List<WheelLine> m_ListWheelLines;

        [Space]
        [Header("SpinWheelConfigs")]
        [SerializeField] private int m_WheelLevel;
        [SerializeField] private SpinWheelConfigs m_SpinWheelConfigs;


        // Events
        private UnityAction onSpinStartEvent;
        private UnityAction<WheelPiece> onSpinEndEvent;


        private bool _isSpinning = false;

        public bool IsSpinning { get { return _isSpinning; } }


        private Vector2 pieceMinSize = new Vector2(81f, 146f);
        private Vector2 pieceMaxSize = new Vector2(144f, 213f);
        private int piecesMin = 2;
        private int piecesMax = 12;

        private float pieceAngle;
        private float halfPieceAngle;
        private float halfPieceAngleWithPaddings;


        private double accumulatedWeight;
        private System.Random rand = new System.Random();

        private List<int> nonZeroChancesIndices = new List<int>();

        private void Awake()
        {
            piecePools.OnInit(wheelPiecePrefab, 10, this.transform);
            linePools.OnInit(linePrefab, 10, this.transform);
        }
        public void StartInGame()
        {

            DrawWheel();
            //SetupAudio () ;

        }
        public void SetupData(int level, SpinWheelConfigs spinWheelConfigs)
        {
            //m_ListWheelPieces.Clear();
            m_ListWheelPiecesData.Clear();
            m_ListWheelPieces.Clear();
            m_ListWheelLines.Clear();
            m_SpinWheelConfigs = spinWheelConfigs;
            m_WheelLevel = level;
            List<WheelDataConfigs<GiftType>> wheelDatas = m_SpinWheelConfigs.SpinWheelDataConfigs[m_WheelLevel].m_WheelDatasConfigs;
            for (int i = 0; i < wheelDatas.Count; i++)
            {
                m_ListWheelPiecesData.Add(wheelDatas[i]);
            }
        }
        //private void SetupAudio () {
        //   audioSource.clip = tickAudioClip ;
        //   audioSource.volume = volume ;
        //   audioSource.pitch = pitch ;
        //}

        private void Generate()
        {
            wheelPiecePrefab = InstantiatePiece();
            RectTransform rt = wheelPiecePrefab.transform.GetChild(0).GetComponent<RectTransform>();
            float pieceWidth = Mathf.Lerp(pieceMinSize.x, pieceMaxSize.x, 1f - Mathf.InverseLerp(piecesMin, piecesMax, m_ListWheelPiecesData.Count));
            float pieceHeight = Mathf.Lerp(pieceMinSize.y, pieceMaxSize.y, 1f - Mathf.InverseLerp(piecesMin, piecesMax, m_ListWheelPiecesData.Count));
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pieceWidth);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, pieceHeight);
            for (int i = 0; i < m_ListWheelPiecesData.Count; i++)
            {
                DrawPiece(i);
            }
            piecePools.Despawn(wheelPiecePrefab);
        }

        public void DrawWheel()
        {
            pieceAngle = 360 / m_ListWheelPiecesData.Count;
            halfPieceAngle = pieceAngle / 2f;
            halfPieceAngleWithPaddings = halfPieceAngle - (halfPieceAngle / 4f);

            Generate();

            CalculateWeightsAndIndices();
            if (nonZeroChancesIndices.Count == 0)
                Debug.LogError("You can't set all pieces chance to zero");

        }
        private void DrawPiece(int index)
        {
            WheelDataConfigs<GiftType> pieceData = m_ListWheelPiecesData[index];
            WheelPiece curPeice = InstantiatePiece();
            curPeice.Setup(pieceData);
            Transform pieceTrns = curPeice.transform.GetChild(0);
            pieceTrns.GetChild(1).GetChild(0).GetComponent<Image>().sprite = pieceData.m_Icon;
            pieceTrns.GetChild(1).GetChild(1).GetComponent<Text>().text = pieceData.m_Amount.ToString();
            m_ListWheelPieces.Add(curPeice);

            //Line
            WheelLine newWheelLine = linePools.Spawn(linesParent.position, Quaternion.identity);
            newWheelLine.transform.parent = linesParent;
            newWheelLine.transform.RotateAround(wheelPiecesParent.position, Vector3.back, (pieceAngle * index) + halfPieceAngle);
            m_ListWheelLines.Add(newWheelLine);
            pieceTrns.RotateAround(wheelPiecesParent.position, Vector3.back, pieceAngle * index);
        }

        private WheelPiece InstantiatePiece()
        {
            WheelPiece newWheelPiece = piecePools.Spawn(wheelPiecesParent.position, Quaternion.identity);
            newWheelPiece.transform.parent = wheelPiecesParent;
            return newWheelPiece;
        }
        public int GetSpinCost()
        {
            return m_SpinWheelConfigs.SpinWheelDataConfigs[m_WheelLevel].m_GemPerSpin;
        }

        public void Spin()
        {
            if (!_isSpinning)
            {
                _isSpinning = true;
                if (onSpinStartEvent != null)
                    onSpinStartEvent.Invoke();

                int index = GetRandomPieceIndex();
                WheelPiece piece = m_ListWheelPieces[index];

                if (piece.m_Chance == 0 && nonZeroChancesIndices.Count != 0)
                {
                    index = nonZeroChancesIndices[Random.Range(0, nonZeroChancesIndices.Count)];
                    piece = m_ListWheelPieces[index];
                }

                float angle = -(pieceAngle * index);

                float rightOffset = (angle - halfPieceAngleWithPaddings) % 360;
                float leftOffset = (angle + halfPieceAngleWithPaddings) % 360;

                float randomAngle = Random.Range(leftOffset, rightOffset);

                Vector3 targetRotation = Vector3.back * (randomAngle + 2 * 360 * spinDuration);

                //float prevAngle = wheelCircle.eulerAngles.z + halfPieceAngle ;
                float prevAngle, currentAngle;
                prevAngle = currentAngle = wheelCircle.eulerAngles.z;

                bool isIndicatorOnTheLine = false;

                wheelCircle
                .DORotate(targetRotation, spinDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.InOutQuart)
                .OnUpdate(() =>
                {
                    float diff = Mathf.Abs(prevAngle - currentAngle);
                    if (diff >= halfPieceAngle)
                    {
                        if (isIndicatorOnTheLine)
                        {
                            //audioSource.PlayOneShot (audioSource.clip) ;
                        }
                        prevAngle = currentAngle;
                        isIndicatorOnTheLine = !isIndicatorOnTheLine;
                    }
                    currentAngle = wheelCircle.eulerAngles.z;
                })
                .OnComplete(() =>
                {
                    _isSpinning = false;
                    if (onSpinEndEvent != null)
                        onSpinEndEvent.Invoke(piece);

                    onSpinStartEvent = null;
                    onSpinEndEvent = null;
                });

            }
        }

        private void FixedUpdate()
        {

        }

        public void OnSpinStart(UnityAction action)
        {
            onSpinStartEvent = action;
        }

        public void OnSpinEnd(UnityAction<WheelPiece> action)
        {
            onSpinEndEvent = action;
        }


        private int GetRandomPieceIndex()
        {
            double r = rand.NextDouble() * accumulatedWeight;

            for (int i = 0; i < m_ListWheelPieces.Count; i++)
                if (m_ListWheelPieces[i]._weight >= r)
                    return i;

            return 0;
        }

        private void CalculateWeightsAndIndices()
        {
            for (int i = 0; i < m_ListWheelPieces.Count; i++)
            {
                WheelPiece piece = m_ListWheelPieces[i];

                //add weights:
                accumulatedWeight += piece.m_Chance;
                piece._weight = accumulatedWeight;

                //add index :
                piece.m_Index = i;

                //save non zero chance indices:
                if (piece.m_Chance > 0)
                    nonZeroChancesIndices.Add(i);
            }
        }

        public void ClearWheel()
        {
            accumulatedWeight = 0;
            foreach (WheelPiece wp in m_ListWheelPieces)
            {
                piecePools.Despawn(wp);
            }
            m_ListWheelPieces.Clear();
            foreach (WheelLine wl in m_ListWheelLines)
            {
                linePools.Despawn(wl);
            }
            wheelCircle.DORotate(Vector3.zero, 0);
            m_ListWheelLines.Clear();
            m_ListWheelPiecesData.Clear();
        }
        private void OnValidate()
        {
            if (PickerWheelTransform != null)
                PickerWheelTransform.localScale = new Vector3(wheelSize, wheelSize, 1f);

            //if (wheelPieces.Length > piecesMax || wheelPieces.Length < piecesMin)
            //    Debug.LogError("[ PickerWheelwheel ]  pieces length must be between " + piecesMin + " and " + piecesMax);
        }
    }
}