import _ from 'lodash';
import React, { Fragment, useState } from 'react';
import SweetAlert from 'react-bootstrap-sweetalert';
import { Button } from 'reactstrap';

const keyMap = {
    save: "ctrl",
};

function ConfirmAlert(props) {
    // const { values } = props
    const [popup, setPopup] = useState(false)

    const openPopup = () => {
        // if (props.formRef.current.dirty && _.isEmpty(props.formRef.current.errors)) {
        //     setPopup(true)
        // }
        setPopup(true)

    }

    const closePopup = () => {
        props.formRef.current.setSubmitting(false)
        setPopup(false)
    }

    return (
        <Fragment>
            <Button className="mb-2 mr-2 btn-icon btn-success1" key='buttonSave' color="success" type="button" onClick={() => openPopup()} name="submit" disabled={props.isSubmitting} >
                <i className="pe-7s-paper-plane btn-icon-wrapper font-weight-bold text-black"> </i> {props.isSubmitting ? "Pleast Wait..." : props.name ? props.name : "Submit"}
            </Button>

            <SweetAlert
                show={popup}
                custom
                showCancel
                showCloseButton
                confirmBtnText="Submit"
                cancelBtnText="Cancel"
                confirmBtnBsStyle="primary"
                cancelBtnBsStyle="light"
                title="Are you sure?"
                style={{ position: 'absolute' }}
                onConfirm={() => { props.handleSave(); setPopup(false) }}
                onCancel={() => closePopup()}
            >

            </SweetAlert>
        </Fragment>
    )
}

export default ConfirmAlert
