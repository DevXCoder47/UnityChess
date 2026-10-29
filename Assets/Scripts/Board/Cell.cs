using Pieces;
using UnityEngine;

namespace Board
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private Piece piece;
        public Piece Piece => piece;

        private void Start()
        {
            if (piece != null)
            {
                Piece piecePrefab = piece;
                SetPiece(Instantiate(piecePrefab, transform.position, transform.rotation));
            }
        }

        public void SetPiece(Piece piece)
        {
            if (piece == null)
            {
                Debug.LogError("Cannot set a null piece on a cell.", this);
                return;
            }

            this.piece = piece;
            this.piece.transform.SetPositionAndRotation(transform.position, transform.rotation);
            PositionPieceOnSurface(this.piece);
        }

        public void RemovePiece()
        {
            piece = null;
        }

        private void PositionPieceOnSurface(Piece pieceToPosition)
        {
            Renderer[] renderers = pieceToPosition.GetComponentsInChildren<Renderer>(true);
            if (renderers.Length == 0)
            {
                Debug.LogError($"Piece '{pieceToPosition.name}' has no renderers to determine its height.", pieceToPosition);
                return;
            }

            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            Collider boardCollider = GetComponentInParent<Collider>();
            Vector3 surfacePosition = transform.position;
            Vector3 surfaceNormal = transform.forward;

            if (boardCollider != null)
            {
                float rayDistance = boardCollider.bounds.extents.magnitude + 1f;
                Ray surfaceRay = new Ray(
                    transform.position + surfaceNormal * rayDistance,
                    -surfaceNormal);

                if (boardCollider.Raycast(surfaceRay, out RaycastHit hit, rayDistance * 2f))
                {
                    surfacePosition = hit.point;
                    surfaceNormal = hit.normal;
                }
                else
                {
                    Debug.LogWarning($"Could not find the board surface for cell '{name}'. Using the cell position.", this);
                }
            }
            else
            {
                Debug.LogWarning($"Cell '{name}' has no parent board collider. Using the cell position.", this);
            }

            Vector3 absoluteNormal = new Vector3(
                Mathf.Abs(surfaceNormal.x),
                Mathf.Abs(surfaceNormal.y),
                Mathf.Abs(surfaceNormal.z));
            float lowestPoint = Vector3.Dot(bounds.center, surfaceNormal)
                - Vector3.Dot(bounds.extents, absoluteNormal);
            float surfaceOffset = Vector3.Dot(surfacePosition, surfaceNormal) - lowestPoint;

            pieceToPosition.transform.position += surfaceNormal * surfaceOffset;
        }
    }
}
