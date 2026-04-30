
import React from 'react';
import { Card, Col, Progress, Row } from 'reactstrap';

 

const PatientGender = (props) => {
    const typeData = props.data.filter((k) => k.type === props.type);
    const json = typeData.length > 0 ? JSON.parse(typeData[0].json) : [];
    const male = json && json.find((k) => k.Gender === 1);
    const female = json && json.find((k) => k.Gender === 2);
    const total = (male ? male.Count : 0) + (female ? female.Count : 0)
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
                                        {(male && male.Count) || 0}
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
                                        Male
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
                                        {(female && female.Count) || 0}
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
                                        Female
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
                                        {total || 0}
                                    </div>
                                </div>
                            </div>
                            <div className="widget-progress-wrapper mt-1">
                                <Progress
                                    className="progress-bar-xs progress-bar-animated-alt"
                                    color="success"
                                    value="83" />
                                <div className="progress-sub-label">
                                    <div className="sub-label-left font-size-md">
                                        Total
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

export default PatientGender;