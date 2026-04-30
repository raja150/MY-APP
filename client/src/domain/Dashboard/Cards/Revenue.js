
import React from 'react';
import { Card, Col, Progress, Row } from 'reactstrap';


const data55 = [
    { name: 'Page A', uv: 4000, pv: 2400, amt: 2400 },
    { name: 'Page B', uv: 3000, pv: 1398, amt: 2210 },
    { name: 'Page C', uv: 2000, pv: 9800, amt: 2290 },
    { name: 'Page D', uv: 2780, pv: 3908, amt: 2000 },
    { name: 'Page E', uv: 1890, pv: 4800, amt: 2181 },
    { name: 'Page F', uv: 2390, pv: 3800, amt: 2500 },
    { name: 'Page G', uv: 3490, pv: 4300, amt: 2100 },
    { name: 'Page C', uv: 2000, pv: 6800, amt: 2290 },
    { name: 'Page D', uv: 4780, pv: 7908, amt: 2000 },
    { name: 'Page E', uv: 2890, pv: 9800, amt: 2181 },
    { name: 'Page F', uv: 1390, pv: 3800, amt: 1500 },
    { name: 'Page G', uv: 3490, pv: 4300, amt: 2100 },
];

const Revenue = (props) => {
    const typeData = props.data.filter((k) => k.type == props.type);
    const json = typeData.length > 0 ? JSON.parse(typeData[0].json) : [];
    return (
        <Card className="main-card p-2">
            <h6>{props.header} </h6>
            {/* <div
                className="widget-chart-wrapper widget-chart-wrapper-lg opacity-10 m-0">
                <ResponsiveContainer width='100%' aspect={3.0 / 1.0}>
                    <LineChart data={data55}
                        margin={{ top: 0, right: 5, left: 5, bottom: 0 }}>
                        <Tooltip />
                        <Line type="monotone" dataKey="pv" stroke="#d6b5ff"
                            strokeWidth={2} />
                        <Line type="monotone" dataKey="uv" stroke="#a75fff"
                            strokeWidth={2} />
                    </LineChart>
                </ResponsiveContainer>
            </div> */}
            <Row>
                <Col md="12" lg="4">
                    <div className="widget-content">
                        <div className="widget-content-outer">
                            <div className="widget-content-wrapper">
                                <div className="widget-content-left">
                                    <div className="widget-numbers text-dark">
                                    {json && json.length > 0 ? json[0].Total : 0}
                                    </div>
                                </div>
                            </div>
                            <div className="widget-progress-wrapper mt-1">
                                <Progress
                                    className="progress-bar-xs progress-bar-animated-alt"
                                    color="info"
                                    value="65" />
                                <div className="progress-sub-label">
                                    <div className="sub-label-left font-size-md">
                                    Service
                                                                </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </Col>
                <Col md="12" lg="4">
                    <div className="widget-content">
                        <div className="widget-content-outer">
                            <div className="widget-content-wrapper">
                                <div className="widget-content-left">
                                    <div className="widget-numbers text-dark">
                                    {json && json.length > 1 ? json[1].Total : 0}
                                    </div>
                                </div>
                            </div>
                            <div className="widget-progress-wrapper mt-1">
                                <Progress
                                    className="progress-bar-xs progress-bar-animated-alt"
                                    color="warning"
                                    value="22" />
                                <div className="progress-sub-label">
                                    <div className="sub-label-left font-size-md">
                                        Pharma
                                                                </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </Col>  
            </Row>
        </Card>
        
    )
}

export default Revenue;