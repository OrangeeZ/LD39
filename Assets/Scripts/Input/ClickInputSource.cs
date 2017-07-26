using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Collections;

public class ClickInputSource : IInputSource {

    public IObservable<Vector3> moveInput { get; private set; }

    public IReadOnlyReactiveProperty<object> targets { get; private set; }

    private Subject<object> targetsSubject = new Subject<object>();

    public ClickInputSource() {

        //moveInput = .

        moveInput = UIJoystick.instance.ObserveEveryValueChanged( _ => _.GetValue() );
        //targets = targetsSubject.ToReadOnlyReactiveProperty( null );

        //UIInputPanel.instance.Subscribe( OnClick );
    }

    private void Update( long ticks ) {

        if ( Input.GetMouseButtonDown( 0 ) ) {

            OnClick( Input.mousePosition );
        }
    }

    private void OnClick( Vector2 point ) {

        var ray = Camera.main.ScreenPointToRay( point );
        var hitInfo = default ( RaycastHit );

        if ( Physics.Raycast( ray, out hitInfo ) ) {

            if ( hitInfo.collider.GetComponent<CharacterPawnBase>() ) {

                targetsSubject.OnNext( hitInfo.collider.GetComponent<CharacterPawnBase>().character );

            } else if ( hitInfo.collider.GetComponent<ItemView>() ) {

                targetsSubject.OnNext( hitInfo.collider.GetComponent<ItemView>() );
            } else {

                targetsSubject.OnNext( hitInfo.point );
            }
        }
    }

}