import React from 'react'
import { Link } from 'react-router-dom'

function GoBack({ link, title, disabledLink }) {
    return (
        <div className="app-page-title pt-2 pb-2 d-md-none d-lg-block">
            <div className="page-title-wrapper">
                <div className="page-title-heading">
                    <div>{title}</div>
                </div>
                {
                    !disabledLink ?
                        <div className="page-title-actions">
                            <Link to={link}>
                                <div href="#" className="btn btn-white btn-hover-primary btn-sm">
                                    Go to list
                                </div>
                            </Link>
                        </div> : ''
                }
            </div>
        </div>
    )
}

export default GoBack
