// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:03:22 by seancooper
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hawksbill.Firebase.Firestore;

[Serializable, CreateAssetMenu (menuName = "Hawksbill/Version")]
public class Version : FirestoreComponent<Version>
{
    public bool forceValidation = false;
    public int maybeInteger = 0;
    public float maybeDouble = 0;

    void OnValidate()
    {
        forceValidation = false;
        load ();
    }

}
