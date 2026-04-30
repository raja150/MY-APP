import React, { Fragment } from 'react';
import PrivateRoute from '../../components/PrivateRoute';
import AppHeader from '../../Layout/AppHeader';
import AppSidebar from '../../Layout/AppSidebar';
import AxPage from './Ax/AxPage';
import BottomTabLayout from './Ax/BottomTabLayout';
import NewTable from './Ax/NewTable';

// import Breadcrumbs from '../Breadcrumbs';

const Dynamic = ({ match }) => {
  
  return (
    <Fragment>
      {/* <ThemeOptions /> */}
      <AppHeader />
      <div className="app-main">
        <AppSidebar />
        <div className="app-main__outer">
          <div className="app-main__inner">
            {/* <Breadcrumbs /> */}
            <PrivateRoute
              path={`${match.url}/list/:id`}
              component={NewTable}
            />
          
            <PrivateRoute path={`${match.url}/screen/`} component={BottomTabLayout} />
            {/* <PrivateRoute path={`${match.url}/show/`} component={AxPage} />
            <PrivateRoute path={`${match.url}/demo`} component={BottomTabLayout} /> */}
           
          </div>
        </div>
      </div>
    </Fragment>
  );
};
export default Dynamic;
