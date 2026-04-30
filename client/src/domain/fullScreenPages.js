import PrivateRoute from 'components/PrivateRoute'
import React, { Fragment } from 'react'
import Test from 'domain/SelfService/Test/index'

function FullScreenPages({ match }) {
    return (
        <Fragment>
            <PrivateRoute path={`${match.url}/test/start`} component={Test} exact />
        </Fragment>
    )
}

export default FullScreenPages
