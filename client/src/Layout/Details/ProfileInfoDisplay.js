import React, { Fragment } from 'react';
import { Button, Col, Row } from 'reactstrap';
const ProfileInfoDisplay = ({ label, info }) => {
    return (
        <div>
            <Row>
                <Col md='3'>{label}</Col>
                <Col md='1'>:</Col>
                <Col md='8'> {info}</Col>
            </Row>
        </div>

    );
}

export default ProfileInfoDisplay