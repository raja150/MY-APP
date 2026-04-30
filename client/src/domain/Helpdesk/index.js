import React, { Fragment } from 'react';
import PrivateRoute from '../../components/PrivateRoute';
import AppHeader from '../../Layout/AppHeader';
import AppSidebar from '../../Layout/AppSidebar';
import TicketsList from './Tickets/List';
import Tickets from './Tickets/Info';
import Ticket from './Tickets/Add';
import EditResponse from './Tickets/Edit';


const HelpDesk = ({ match }) => (
    <Fragment>
        <AppHeader />
        <div className="app-main">
            <AppSidebar />
            <div className="app-main__outer">
                <div className="app-main__inner">
                    <PrivateRoute path={`${match.url}/Tickets`} component={TicketsList} exact />
                    <PrivateRoute path={`${match.url}/Tickets/Update`} component={Tickets} exact />
                    <PrivateRoute path={`${match.url}/Tickets/New`} component={Ticket} exact />
                    <PrivateRoute path={`${match.url}/Tickets/Edit`} component={EditResponse} exact />
                </div>
            </div>
        </div>
    </Fragment>
);

export default HelpDesk;
