using UnityEngine;

public enum HandSideType {Left, Right, None}

public class HandSide: MonoBehaviour
{
    public HandSideType handSide = HandSideType.Left;
}
