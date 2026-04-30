import React from 'react'
import PropTypes from 'prop-types'
import {FormFeedback} from 'reactstrap'

export default class Error extends React.Component {
    static propTypes = {
        touched: PropTypes.bool,
        error: PropTypes.string
    }

    render() { 
        const { touched, error } = this.props
        return touched && error ? (
            <FormFeedback>
                {error}
            </FormFeedback>
        ) : null
    }
}
