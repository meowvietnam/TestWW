using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxPool : PoolManager
{

    private static VfxPool instance;
    public static VfxPool Instance { get => instance; }
    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }
    // Start is called before the first frame update



  
    public void SetVfxCoin(Vector2 pos)
    {
        ActivePoolObject(0);
        GameObject vfxObj = poolObject;
        vfxObj.transform.SetParent(BoardManager.Instance.boardGameObject.transform);
        vfxObj.transform.localPosition = pos;
        vfxObj.transform.DOLocalJump(new Vector3(vfxObj.transform.localPosition.x, vfxObj.transform.localPosition.y + 0.5f, vfxObj.transform.localPosition.z), 1f, 1, 0.5f).OnComplete(() =>
        {
          

         
            Vector3 localTargetPosition = vfxObj.transform.parent.InverseTransformPoint(GameManager.Instance.scoreText.transform.position);

            vfxObj.transform.DOLocalMove(localTargetPosition, 1f).OnComplete(() =>
            {
                DestroyAfterEvent(vfxObj, 0);
            });

        });
    }    
}
