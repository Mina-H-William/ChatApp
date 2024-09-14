import React from "react";
import { Container, Row, Col, Form, Button } from "react-bootstrap";
import loginImg from "../assets/chat-image.png";
export default function Login() {
  return (
    <Container>
      <Row>
        <Col>
          <h1>Login</h1>
          <Form>
            <Form.Group controlId="formBasicEmail">
              <Form.Label>Email address</Form.Label>
              <Form.Control type="email" placeholder="Enter email" />
              <Form.Text className="text-muted">
                We'll never share your email with anyone else.
              </Form.Text>
            </Form.Group>
            <Form.Group controlId="formBasicPassword">
              <Form.Label>Password</Form.Label>
              <Form.Control type="password" placeholder="Password" />
            </Form.Group>
            <Button variant="primary" type="submit">
              Submit
            </Button>
          </Form>
        </Col>
        <Col>
          <img src={loginImg} alt="login" />
        </Col>
      </Row>
    </Container>
  );
}
