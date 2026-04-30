import React from 'react';
import { Route, Redirect } from 'react-router-dom';
import ErrorBoundary from '../domain/Error/ErrorBoundary';
import SessionStorageService from 'services/SessionStorage';

const PrivateRoute = ({component: Component, ...rest}) => { 
    return (

        // Show the component only when the user is logged in
        // Otherwise, redirect the user to /signin page
        <Route {...rest} render={props => (
            SessionStorageService.getUser() ?
                <ErrorBoundary>
                <Component {...props} />
                </ErrorBoundary>
            : <Redirect to="/login" />
        )} />
    );
};

export default PrivateRoute;