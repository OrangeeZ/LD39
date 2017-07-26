using UnityEngine;
using UnityEngine.EventSystems;

public class UIJoystick : AObject, IBeginDragHandler, IEndDragHandler, IDragHandler {

    public enum Mode {

        XY,
        XZ,

    }

//#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    public bool simulateInput = true;
    public bool alternativeMode = true;
//#endif

    public static UIJoystick instance { get; private set; }

    [SerializeField]
    private bool _normalize;

    [SerializeField]
    private Mode mode;

    [SerializeField]
    private float radius = 5f;

    [SerializeField]
    private float resetSpeed = 0.1f;

    [SerializeField]
    private Transform root;

    private bool isDragging;

    private void Start() {

        instance = this;
    }

    private void LateUpdate() {

        if ( !isDragging ) {

            localPosition = Vector3.Lerp( localPosition, Vector3.zero, resetSpeed );
        } else {

            var currentVector = Vector3.ClampMagnitude( localPosition, radius );
            localPosition = currentVector;
        }
    }

    public Vector3 GetValue() {

//#if UNITY_EDITOR || UNITY_STANDALONE_WIN 
        //if ( simulateInput ) {

        var simulatedInput = alternativeMode ?
            MakeVector( Input.GetAxis( "Horizontal Alt" ), Input.GetAxis( "Vertical Alt" ) ) :
            MakeVector( Input.GetAxis( "Horizontal" ), Input.GetAxis( "Vertical" ) );
        //}
//#endif
        var result = Vector3.ClampMagnitude( ( position - root.position ) / radius, 1f );

        result = MakeVector( result.x, result.y ) + simulatedInput;
        
        return _normalize ? result.normalized : result;
    }

    public void OnBeginDrag( PointerEventData eventData ) {

        isDragging = true;
    }

    public void OnEndDrag( PointerEventData eventData ) {

        isDragging = false;
    }

    public void OnDrag( PointerEventData eventData ) {

        position = eventData.position;
    }

    private Vector3 MakeVector( float horizontal, float vertical ) {

        switch ( mode ) {

            case Mode.XZ:
                return new Vector3( horizontal, 0, vertical );

            case Mode.XY:
            default:
                return new Vector3( horizontal, vertical );
        }
    }

}