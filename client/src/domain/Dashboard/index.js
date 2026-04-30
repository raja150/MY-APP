import Details from 'Layout/Details';
import React, { Fragment } from 'react';
import PrivateRoute from '../../components/PrivateRoute';
// Layout
import AppHeader from '../../Layout/AppHeader';
import AppSidebar from '../../Layout/AppSidebar';
// import AppFooter from '../../Layout/AppFooter/';
// Theme Options
import ThemeOptions from '../../Layout/ThemeOptions';
import CalendarDetails from './CalendarDetails';
// import Admin from './Admin';
// import Biller from './Biller';
// import Doctor from './Doctor';
// import Reception from './Reception';
import User from './User';

const Dashboards = ({ match }) => (
    <Fragment>
        <ThemeOptions />
        <AppHeader />
        <div className="app-main">
            <AppSidebar />
            <div className="app-main__outer">
                <div className="app-main__inner">
                    <PrivateRoute path={`${match.url}/User`} component={User} />
                    <PrivateRoute path={`${match.url}/Details`} component={Details} />
                    <PrivateRoute path={`${match.url}/CalendarDetails`} component={CalendarDetails} />
                    {/* <PrivateRoute path={`${match.url}/Reception`} component={Reception} />
                    <PrivateRoute path={`${match.url}/Biller`} component={Biller} />
                    <PrivateRoute path={`${match.url}/Doctor`} component={Doctor} /> */}
                </div>
               
            </div>
        </div>
    </Fragment>
);

export default Dashboards;