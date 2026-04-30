

import React from 'react';

function PageHeader({ title }) {
    return (<div className="app-page-title pt-2 pb-2">
        <div className="page-title-wrapper">
            <div className="page-title-heading">
                <div>{title}</div>
            </div>
        </div>
    </div>
    )
}
export default PageHeader