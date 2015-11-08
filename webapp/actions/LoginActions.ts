﻿import {Action, typeName} from './Actions';
import {ActionTypes} from './ActionTypes';
import {AppState} from './../app/AppState';
import {handleActions} from 'redux-actions';
import {owl} from './../utils/deepCopy';


@typeName(ActionTypes[ActionTypes.LOGIN_USER])
export class LoginAction extends Action {
    private _brand: void;

    constructor(public jwt: string) {
        super();
    }
}

@typeName(ActionTypes[ActionTypes.LOGOUT_USER])
export class LogoutAction extends Action {
    private _brand: void;

    constructor() {
        super();
    }
}

export var LoginReducer = handleActions<AppState>(
    {
        [ActionTypes[ActionTypes.LOGIN_USER]]: (state: AppState, action: LoginAction): AppState => {
            var result = owl.clone(state);

            var savedJwt = localStorage.getItem('jwt');

            if (savedJwt !== action.jwt) {
                //var nextPath = RouterContainer.get().getCurrentQuery().nextPath || '/';

                //RouterContainer.get().transitionTo(nextPath);
                localStorage.setItem('jwt', action.jwt);
            }

            result.jwt = action.jwt;
            return result;
        },
        [ActionTypes[ActionTypes.LOGOUT_USER]]: (state: AppState, action: LogoutAction): AppState => {
            var result = owl.clone(state);

            localStorage.removeItem('jwt');

            result.jwt = undefined;
            return result;
        }
    }, new AppState()
);