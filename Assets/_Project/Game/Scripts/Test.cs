using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Test : MonoBehaviour
{
    private string _signUpPopUp = "WindowLogIn";

    [SerializeField] private Transform _mainCanvas;
    private async void LoadPopUp()
    {
        await Addressables.LoadAssetAsync<GameObject>(_signUpPopUp).Task;
        GameObject go = await Addressables.InstantiateAsync(_signUpPopUp, _mainCanvas).Task;

    }
}
