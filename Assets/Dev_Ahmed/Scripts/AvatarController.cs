using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour
{
    public Transform AvatarTranformPosition;
    public Transform AvatarHead;
    public Transform Avatarbody;
   
    public Transform Avatar_Hand_Left;
    public Transform Avatar_hand_Right;
   
    public Transform XRHead;
    public Transform XRHand_Left;
    public Transform XRHand_right;

    public Vector3 XRHead_Position_Offset;
    public Vector3 Hand_Rotation_Offset;




    private void Update()
    {
        AvatarTranformPosition.transform.position = Vector3.Lerp(AvatarTranformPosition.position, XRHead.transform.position + XRHead_Position_Offset , 0.5f);
        AvatarHead.rotation = Quaternion.Lerp(AvatarHead.rotation ,XRHead.rotation,0.5f);
        Avatarbody.rotation = Quaternion.Lerp(Avatarbody.rotation, Quaternion.Euler(new Vector3(0, AvatarHead.rotation.eulerAngles.y, 0)), 0.05f);



        Avatar_Hand_Left.position = Vector3.Lerp(Avatar_Hand_Left.position, XRHand_Left.position, 0.5f);
        Avatar_hand_Right.position = Vector3.Lerp(Avatar_hand_Right.position, XRHand_right.position, 0.5f);


        Avatar_Hand_Left.rotation = Quaternion.Lerp(Avatar_Hand_Left.rotation, XRHand_Left.rotation, 0.5f) * Quaternion.Euler(Hand_Rotation_Offset);
        Avatar_hand_Right.rotation = Quaternion.Lerp(Avatar_hand_Right.rotation, XRHand_right.rotation, 0.5f) * Quaternion.Euler(Hand_Rotation_Offset);

    }



}
