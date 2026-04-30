import { BrowserRouter as Router, Route, Redirect } from 'react-router-dom';
import React, { Suspense, lazy, Fragment } from 'react';
import Loader from 'react-loaders'
import PrivateRoute from '../../components/PrivateRoute';
import { ToastContainer } from 'react-toastify';
import AppHeader from 'Layout/AppHeader';
import AppSidebar from 'Layout/AppSidebar';
import Details from 'Layout/Details';
import FullScreenPages from 'domain/fullScreenPages';
//import PatientData from 'domain/Nurshing/PatientData.js';

const DataForm = lazy(() => import('../../domain/DataForm'));
const Dashboard = lazy(() => import('../../domain/Dashboard'));
const Domain = lazy(() => import('../../domain'));

const ChangePassword = lazy(() => import('../ChangePassword'));


const NotFound = () =>
    <div>
        {/* <h3>404 page not found</h3>
    <p>We are sorry but the page you are looking for does not exist.</p> */}
    </div>

const AppMain = () => {

    return (
        <Fragment>


            <Suspense fallback={
                <div className="loader-container">
                    <div className="loader-container-inner">
                        <div className="text-center">
                            <Loader type="line-scale-party" />
                        </div>
                        <h6 className="mt-3">
                            Please wait while we load the Page
                        </h6>
                    </div>
                </div>
            }>

                <Route path="/d" component={DataForm} />
            </Suspense>

            <Suspense fallback={
                <div className="loader-container">
                    <div className="loader-container-inner">
                        <div className="text-center">
                            <Loader type="line-scale-party" />
                        </div>
                        <h6 className="mt-3">
                            Please wait while we load the Page
                        </h6>
                    </div>
                </div>
            }>
                <Route path="/m" component={Domain} />
            </Suspense>

            <Suspense fallback={
                <div className="loader-container">
                    <div className="loader-container-inner">
                        <div className="text-center">
                            <Loader type="line-scale-party" />
                        </div>
                        <h6 className="mt-3">
                            Please wait while we load the Page
                        </h6>
                    </div>
                </div>
            }>
                <Route path="/f" component={FullScreenPages} />
            </Suspense>

            <Suspense fallback={
                <div className="loader-container">
                    <div className="loader-container-inner">
                        <div className="text-center">
                            <Loader type="line-scale-party" />
                        </div>
                        <h6 className="mt-3">
                            Please wait while we load the Page
                        </h6>
                    </div>
                </div>
            }>
                <Route path="/n" component={Domain} />
            </Suspense>

            {/* Dashboards */}
            <Suspense fallback={
                <div className="loader-container">
                    <div className="loader-container-inner">
                        <div className="text-center">
                            <Loader type="ball-grid-beat" />
                        </div>
                        <h6 className="mt-3">
                            Please wait while we load the Page
                        </h6>
                    </div>
                </div>
            }>
                <PrivateRoute path="/Dashboard" component={Dashboard} />
                <PrivateRoute path="/changePassword" component={ChangePassword} />
                {/* <PrivateRoute path="/Details" component={Details} /> */}
            </Suspense>

            {/* Print */}
            {/* <Suspense fallback={
                <div className="loader-container">
                    <div className="loader-container-inner">
                        <div className="text-center">
                            <Loader type="line-scale-party" />
                        </div>
                        <h6 className="mt-3">
                            Please wait while we load the Page
                            </h6>
                    </div>
                </div>
            }>
                <Route path="/Print" component={Print} />
            </Suspense> */}

            {/* Roles */}
            {/* <Suspense fallback={
                <div className="loader-container">
                    <div className="loader-container-inner">
                        <div className="text-center">
                            <Loader type="ball-grid-beat" />
                        </div>
                        <h6 className="mt-3">
                            Please wait while we load the Page
                        </h6>
                    </div>
                </div>
            }>
                <PrivateRoute path="/Roles" component={Role} />
            </Suspense> */}


            {/* Reports */}
            {/* <Suspense fallback={
                <div className="loader-container">
                    <div className="loader-container-inner">
                        <div className="text-center">
                            <Loader type="ball-grid-beat" />
                        </div>
                        <h6 className="mt-3">
                            Please wait while we load the Page
                        </h6>
                    </div>
                </div>
            }>
                <PrivateRoute path="/Reports" component={Reports} />
            </Suspense> */}

            <Route exact path="/" render={() => (
                <Redirect to="/Dashboard/user" />
            )} />

            <Route path="*" component={NotFound} />
            <ToastContainer />



        </Fragment>
    )
};

export default AppMain;