using Mirror;
using TDGame.Cursor;
using TDGame.Events.Base;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TDGame.Building
{
    public class SelectionController : MonoBehaviour
    {
        private Camera referenceCamera;

        private GameObject selectedTower;

        [SerializeField]
        private LocalCursorState cursorState;

        [SerializeField]
        private GameEvent<GameObject> gameEvent;

        private void Start()
        {
            referenceCamera = Camera.main;
        }

        void Update()
        {
            Ray ray = referenceCamera.ScreenPointToRay(Input.mousePosition);
            if (Input.GetMouseButtonDown(0) && cursorState.State == CursorState.None &&
                Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Tower")))
            {
                GameObject hitPoint = hit.collider.gameObject;
                FindTowerCoreRecursive(hitPoint.transform);
            }
            else if (Input.GetMouseButtonDown(1) || (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()))
            {
                CheckIfAlreadySelecting();
            }
        }

        public void ChangeSelection(GameObject selection)
        {
            selectedTower = selection;
            gameEvent.Raise(selectedTower);
        }

        private void CheckIfAlreadySelecting()
        {
            if (selectedTower != null)
            {
                ChangeSelection(null);
            }
        }

        private void FindTowerCoreRecursive(Transform transformToCheck)
        {
            if (transformToCheck.GetComponent(typeof(NetworkIdentity)))
            {
                if (selectedTower == transformToCheck.gameObject)
                    return;

                CheckIfAlreadySelecting();

                ChangeSelection(transformToCheck.gameObject);
            }
            else
                FindTowerCoreRecursive(transformToCheck.parent);
        }
    }
}