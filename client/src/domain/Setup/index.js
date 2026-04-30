import React, { Fragment } from 'react';
import AppHeader from '../../Layout/AppHeader';
import AppSidebar from '../../Layout/AppSidebar';

const Pharmacy = ({ match }) => (
  <Fragment>
    {/* <ThemeOptions /> */}
    <AppHeader />
    <div className="app-main">
      <AppSidebar />
      <div className="app-main__outer">
        <div className="app-main__inner">

        {/* <PrivateRoute path={`${match.url}/Import`} component={DataImport} exact /> */}

        </div>
      </div>
    </div>
  </Fragment>
);

export default Pharmacy;
