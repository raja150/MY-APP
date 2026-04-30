import React, { Fragment } from 'react';
import { Button, Col, Row } from 'reactstrap';
const InfoDisplay = ({ label, info }) => {

    return (
        <Fragment>
            <Row>
                <Col md='5'>
                    <h6>{label}</h6>
                </Col>
                <Col md='1'>
                    <h6>:</h6>
                </Col>
                <Col md='6'>
                    <h6>{info}</h6>
                </Col>
            </Row>
        </Fragment>
    );
}

export default InfoDisplay