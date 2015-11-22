﻿import * as React from 'react';
import {Navigation} from './Navigation';
import ReactRouter = require('react-router');
import {KernelCreator} from './kernel';
import * as TypeIoc from 'typeioc';
import {IKernelContext} from './IKernelContext';
import {IAuthServiceContext, IAuthService} from './../api/AuthService';

import { bindActionCreators } from 'redux';

var RouteHandler = ReactRouter.RouteHandler;

var kernelSingleton = KernelCreator.create();

export class App extends React.Component<any, any> implements React.ChildContextProvider<IAuthServiceContext> {

    static childContextTypes: React.ValidationMap<any> = {
        authService: React.PropTypes.object
    };

    getChildContext(): IAuthServiceContext {
        return {
            authService: kernelSingleton.resolve<IAuthService>("IAuthService")
        };
    };

    render(): JSX.Element {
        const { state, dispatch } = this.props;

        return (
            <div>
                <Navigation/>
                <div className="container">
                <RouteHandler/>
                </div>
            </div>
        );
    }
}