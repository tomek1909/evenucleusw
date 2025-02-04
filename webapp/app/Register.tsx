﻿import * as React from 'react';

import {IAuthServiceContext} from './../api/AuthService';
import {IStoreContext} from './IStoreContext';
import {IRouterContext} from './IRouterContext';
import {createRegisterAction} from './../actions/LoginActions';
import {IApiContext} from './IApiContext';
import {createNotificationShowAction, NotificationType} from './../actions/NotificationActions';


var Input = require('./../forms/input');
var LinkedStateMixin  = require('react/lib/LinkedStateMixin');

require('app/Login.css');

import Formsy = require('formsy-react');


class RegisterState {
    formError: string;
    canSubmit: boolean;

    constructor() {
        this.formError = '';
        this.canSubmit = false;
    }
}

class RegisterModel {
    email: string;
    password: string;
    password2: string;
}

export class Register extends React.Component<any, RegisterState> {
    static mixins = [LinkedStateMixin];

    constructor() {
        super();
        this.state = new RegisterState();
    }

    context: IStoreContext & IRouterContext & IAuthServiceContext & IApiContext;

    static contextTypes: React.ValidationMap<any> = {
        history: React.PropTypes.object.isRequired,
        store: React.PropTypes.object.isRequired,
        authService: React.PropTypes.object.isRequired,
        api: React.PropTypes.object.isRequired
    };

    enableButton() {
        this.setState(
            (prevState: RegisterState, props: any): RegisterState => {
                prevState.canSubmit = true;
                return prevState;
            })
    }

    disableButton() {
        this.setState(
            (prevState: RegisterState, props: any): RegisterState => {
                prevState.canSubmit = false;
                return prevState;
            })
    }

    submit(model: RegisterModel): When.Promise<void> {
        var that = this;
        return this.context.authService.register(model.email, model.password)
            .then(function (jwt: string) {
                that.context.store.dispatch(createRegisterAction(that.context.api, jwt, model.email));
                that.context.history.pushState('/');
            })
            .catch(function(err) {
                console.log("Error logging in", err);
                that.context.store.dispatch(createNotificationShowAction(NotificationType.error, "error", err.errorMessage));
                that.setState((prevState: RegisterState, props: any): RegisterState => {
                    prevState.formError = err.errorMessage;
                    return prevState;
                });
            });
    }

    render(): JSX.Element {
        var that = this;
        var submitProxy = function (model: RegisterModel) {
            that.submit(model);
        };

        return (
            <div className="row">
                <div className="col-sm-6 col-md-4 col-md-offset-4">
                    <h1 className="text-center login-title">Please register</h1>
                    <div className="account-wall">
                        <Formsy.Form className="form-signin" onValidSubmit={submitProxy.bind(this)} onValid={this.enableButton.bind(this)} onInvalid={this.disableButton.bind(this)}>
                            <span className="help-block" ref="helpblock">{this.state.formError}</span>
                            <Input name="email" type="text" validations="isEmail" placeholder="Email" required autofocus layout="elementOnly" validationError="This is not a valid email" ref="email"></Input>
                            <Input name="password" type="password" placeholder="Password" required layout="elementOnly" ref="password"></Input>
                            <Input name="password2" type="password" placeholder="Confirm password" required layout="elementOnly" ref="password2"></Input>
                            <button className="btn btn-lg btn-primary btn-block" type="submit" disabled={!this.state.canSubmit} ref="button">
                                Register</button>
                            <label className="checkbox pull-left">
                                <input type="checkbox" value="remember-me"></input>
                                Remember me
                            </label>
                        </Formsy.Form>
                    </div>                    
                </div>
            </div>
        );
    }

}


