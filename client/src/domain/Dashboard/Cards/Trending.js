
import React from 'react';
import { Button, Card, CardBody } from 'reactstrap';

const Trending = (props) => {
    const typeData = props.data.filter((k) => k.type === props.type);
    const json = typeData.length > 0 ? JSON.parse(typeData[0].json) : []; 
    const getColor = (idx) => {
        if (idx === 0) {
            return "primary";
        } else if (idx === 1) {
            return "warning";
        } else {
            return "success";
        }
    }

    return (
        <Card className="main-card p-2">
            <h6>{props.header} </h6>
            <CardBody className="p-2 button-new">
                {json && json.length>0 ? json.map((item, k) => { 
                    return <Button className="mb-2 mr-1 p-1" color={getColor(k)} key={k}>
                        {item.Name}
                        <span className="badge badge-pill badge-light">{item.Count}</span>
                    </Button>
                }) : <Button className="mb-2 mr-1 p-1" color={getColor(0)} key={1}> {'-'}
                <span className="badge badge-pill badge-light">0</span>
            </Button>}
            </CardBody>
        </Card>
    )
}

export default Trending;