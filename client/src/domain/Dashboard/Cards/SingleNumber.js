
import React from 'react';
import { Button, Card, CardBody } from 'reactstrap';

const SingleNumber = (props) => { 
    const typeData = props.data.filter((k) => k.type === props.type);
    const json = typeData.length > 0 ? JSON.parse(typeData[0].json) : {};
    
    return ( 
        <Card className="main-card p-2">
        <h6>{props.header} </h6>
        <CardBody className="p-2 button-new">
            <Button className="mb-2 mr-1 p-1" color="primary">
                {props.label}
                <span className="badge badge-pill badge-light">{(json && json[0] && json[0].Value) || 0}</span>
            </Button> 
        </CardBody>
    </Card>
    )
}

export default SingleNumber;