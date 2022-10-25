using UnityEngine;
using UnityEngine.Events;

namespace ProjectHexGrid.Scripts.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private bool movementByPan = true;

        [SerializeField] private float speedMovementByKeys = 5.0f;
        [SerializeField] private bool movementByKeys = true;

        [SerializeField] private float speedScrollByWheel = 500.0f;
        [SerializeField] private bool scrollByWheel = true;

        [SerializeField] private float speedScrollByKeys = 5.0f;
        [SerializeField] private bool scrollByKeys = true;

        [SerializeField] private float scrollSizeMin = 1.0f;
        [SerializeField] private float scrollSizeMax = 3.0f;

        public UnityEvent<Vector3> onClick;

        private UnityEngine.Camera _camera;

        private Vector3 _pivotMouse = Vector3.zero;
        private bool _isDrag = false;

        private void Awake()
        {
            _camera = GetComponent<UnityEngine.Camera>();

            _camera.orthographicSize = scrollSizeMax;
        }

        private void Update()
        {
            if(scrollByWheel)
                Scrolling_byWheel();

            if(scrollByKeys)
                Scrolling_byKeys();

            if (movementByKeys)
                MovementByKeys();

            if (movementByPan)
                MovementByPan();

            ClickHandling();
        }

        private void ClickHandling()
        {
            if(Input.GetMouseButtonDown(0))
            {
                Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                onClick?.Invoke(mouseWorldPosition);
            }
        }

        private void MovementByKeys()
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");

            Vector3 deltaMovement = new Vector3(moveX, moveY, 0.0f) * (speedMovementByKeys * Time.deltaTime);

            transform.position += deltaMovement;
        }

        private void MovementByPan()
        {
            if(Input.GetMouseButtonDown(2))
            {
                // Click mouse position
                _pivotMouse = _camera.ScreenToWorldPoint(Input.mousePosition);
                _isDrag = true;
            }

            if(Input.GetMouseButtonUp(2))
            {
                _isDrag = false;
            }

            if(_isDrag)
            {
                // difference between mouse position and camera position
                Vector3 deltaDragMouse = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

                // first mouse clicked position minus delta mouse position and camera position
                transform.position = _pivotMouse - deltaDragMouse;
            }
        }

        private void Scrolling_byWheel()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel") * -1.0f;

            float deltaScroll = scroll * speedScrollByWheel * Time.deltaTime;
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize + deltaScroll, scrollSizeMin, scrollSizeMax);
        }

        private void Scrolling_byKeys()
        {
            float scroll = 0.0f;

            if(Input.GetKey(KeyCode.E))
            {
                scroll = 1.0f;
            }

            if(Input.GetKey(KeyCode.Q) && scroll == 0.0f)
            {
                scroll = -1.0f;
            }

            float deltaScroll = scroll * speedScrollByKeys * Time.deltaTime;
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize + deltaScroll, scrollSizeMin, scrollSizeMax);
        }
    }
}
