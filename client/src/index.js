import 'bootstrap/dist/css/bootstrap.css';
import { createBrowserHistory } from 'history';
import ExpirePassword from 'Layout/ChangePassword/expirePwd';
import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { HashRouter, Route } from 'react-router-dom';
import './assets/base.scss';
import configureStore from './config/configureStore';
import CheckUser from './domain/UserPages/LoginBoxed/CheckUser';
import LoginBoxed from './domain/UserPages/LoginBoxed/index1';
import ResetPwd from './domain/UserPages/LoginBoxed/ResetPwd';
import Main from './Main';
import './polyfills';
import * as serviceWorker from './serviceWorker';


const store = configureStore();
const rootElement = document.getElementById('root');
export const history = createBrowserHistory();

const renderApp = (Component) => {
  ReactDOM.render(
    <Provider store={store}>
      <HashRouter history={history}>
        <Route path="/login" component={LoginBoxed} />
        <Route path="/forgot" component={CheckUser } />
        <Route path="/reset" component={ResetPwd } />
        <Route path="/ExpirePassword" component={ExpirePassword } />
        <Component />
      </HashRouter>
    </Provider>,
    rootElement
  );
};

renderApp(Main);

// if (module.hot) {
//   module.hot.accept('./Pages/Main', () => {
//     const NextApp = require('./Pages/Main').default;
//     renderApp(NextApp);
//   });
// }
serviceWorker.unregister();
