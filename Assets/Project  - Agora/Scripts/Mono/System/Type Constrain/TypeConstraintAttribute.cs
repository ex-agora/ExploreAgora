using UnityEngine;

public class TypeConstraintAttribute : PropertyAttribute
{
    #region Fields
    private System.Type type;
    #endregion Fields

    #region Constructors
    public TypeConstraintAttribute(System.Type type) { this.type = type; }
    #endregion Constructors

    #region Properties
    public System.Type Type { get => type; }
    #endregion Properties
}