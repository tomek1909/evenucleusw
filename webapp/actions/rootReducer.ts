﻿import { combineReducers } from 'redux'; 
import {LoginReducer} from './LoginActions'; 
import {NotificationReducer} from './NotificationActions'; 
import {KeyReducer} from './KeyActions'; 
import {ConfirmReducer} from './ConfirmActions'; 
import {PilotsReducer} from './PilotsActions'; 
 
 
const rootReducer = combineReducers({
    loginInfo: LoginReducer,
    notifications: NotificationReducer,
    keys: KeyReducer,
    confirmInfo: ConfirmReducer,
    pilots: PilotsReducer,
}); 
 
export { rootReducer }; 
