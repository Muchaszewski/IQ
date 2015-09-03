using UnityEngine;

public static class TransformUtils
{
    #region Position
    /// <summary>
    /// Set position x of current transform
    /// </summary>
    public static Transform SetX(this Transform transform, float x)
    {
        var newTransform = new Vector3(x, transform.position.y, transform.position.z);
        transform.position = newTransform;
        return transform;
    }

    /// <summary>
    /// Add position x of current transform to the current position
    /// </summary>
    public static Transform AddX(this Transform transform, float x)
    {
        var newTransform = new Vector3(transform.position.x + x, transform.position.y, transform.position.z);
        transform.position = newTransform;
        return transform;
    }

    /// <summary>
    /// Set position y of current transform
    /// </summary>
    public static Transform SetY(this Transform transform, float y)
    {
        var newTransform = new Vector3(transform.position.x, y, transform.position.z);
        transform.position = newTransform;
        return transform;
    }

    /// <summary>
    /// Add position y of current transform to the current position
    /// </summary>
    public static Transform AddY(this Transform transform, float y)
    {
        var newTransform = new Vector3(transform.position.x, transform.position.y + y, transform.position.z);
        transform.position = newTransform;
        return transform;
    }

    /// <summary>
    /// Set position z of current transform
    /// </summary>
    public static Transform SetZ(this Transform transform, float z)
    {
        var newTransform = new Vector3(transform.position.x, transform.position.y, z);
        transform.position = newTransform;
        return transform;
    }

    /// <summary>
    /// Add position z of current transform to the current position
    /// </summary>
    public static Transform AddZ(this Transform transform, float z)
    {
        var newTransform = new Vector3(transform.position.x, transform.position.y, transform.position.z + z);
        transform.position = newTransform;
        return transform;
    }

    /// <summary>
    /// Set position x and y of current transform
    /// </summary>
    public static Transform Set2D(this Transform transform, float x, float y)
    {
        var newTransform = new Vector3(x, y, transform.position.z);
        transform.position = newTransform;
        return transform;
    }

    /// <summary>
    /// Add position x and y of current transform to the current position
    /// </summary>
    public static Transform Add2D(this Transform transform, float x, float y)
    {
        var newTransform = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);
        transform.position = newTransform;
        return transform;
    }

    public static Transform AddScale(this Transform transform, Vector3 vector)
    {
        var newTransform = new Vector3(transform.localScale.x + vector.x, transform.localScale.y + vector.y, transform.localScale.z + vector.z);
        transform.localScale = newTransform;
        return transform;
    }

    #endregion

    #region Rotation
    public static Transform SetRotationX(this Transform transform, float x)
    {
        var newTransform = new Vector3(transform.localRotation.x + x, transform.localRotation.y, transform.localRotation.z);
        transform.position = newTransform;
        return transform;
    }
    public static Transform AddRotationX(this Transform transform, float x)
    {
        var newTransform = new Vector3(transform.localRotation.x + x, transform.localRotation.y, transform.localRotation.z);
        transform.position = newTransform;
        return transform;
    }

    public static Transform SetRotationY(this Transform transform, float y)
    {
        var newTransform = new Vector3(transform.localRotation.x, transform.localRotation.y + y, transform.localRotation.z);
        transform.position = newTransform;
        return transform;
    }
    public static Transform AddRotationY(this Transform transform, float y)
    {
        var newTransform = new Vector3(transform.localRotation.x, transform.localRotation.y + y, transform.localRotation.z);
        transform.position = newTransform;
        return transform;
    }

    public static Transform SetRotationZ(this Transform transform, float z)
    {
        var newTransform = new Vector3(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z + z);
        transform.position = newTransform;
        return transform;
    }
    public static Transform AddRotationZ(this Transform transform, float z)
    {
        var newTransform = new Vector3(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z + z);
        transform.position = newTransform;
        return transform;
    }

    /// <summary>
    /// Look at target Vector in world space
    /// </summary>
    /// <param name="inputVector">Vector to look at</param>
    /// <returns></returns>
    public static Transform LookAt2D(this Transform transform, Vector2 inputVector)
    {
        //Get this object point absolutive
        Vector3 object_pos = transform.position;
        //Calculate diffrence
        inputVector.x = inputVector.x - object_pos.x;
        inputVector.y = inputVector.y - object_pos.y;
        //Get angle
        float angle = Mathf.Atan2(inputVector.y, inputVector.x) * Mathf.Rad2Deg;
        //Set the rotation
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        return transform;
    }
    #endregion

    #region Random
    public static bool RandomBool()
    {
        return (Random.value > 0.5f);
    }

    /// <summary>
    /// Return true if random value is lesser then chance
    /// </summary>
    /// <param name="chance">Value between 0f and 1f</param>
    /// <returns></returns>
    public static bool RandomChance(float chance)
    {
        return (Random.value > chance);
    }
    #endregion
}

