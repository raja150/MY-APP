import React from 'react';
import { Row, Col } from 'reactstrap';
import * as date from 'utils/date';

export default function HeaderComponent(props) {
    return (
        <div>
            <Row>
                <Col md='4'>{props.name}</Col>
            </Row>
            <Row style={{ fontSize: '15px' }}>
                <Col md='6'>{date.getRequiredDate(props.date)}</Col>
            </Row>
        </div>
    )
}