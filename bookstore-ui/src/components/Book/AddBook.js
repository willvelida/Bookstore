import React from 'react';
import axios from 'axios';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

export default class AddBook extends React.Component {
    constructor() {
        super();
        this.state = {
            BookName: '',
            Price: '',
            Category: '',
            Author: ''
        };
    }

    handleChange = event => {
        const field = event.target.id;
        if (field === "BookName") {
            this.setState({ BookName: event.target.value });
        } else if (field === "Price") {
            this.setState({ Price: event.target.value });
        } else if (field === "Category") {
            this.setState({ Category: event.target.value });
        } else if (field === "Author") {
            this.setState({ Author: event.target.value });
        }
    }

    handleSubmit = event => {
        event.preventDefault();
        axios({
            method: "POST",
            url: "https://favelidabookstoreapi.azurewebsites.net/api/Book",
            data: this.state
        }).then((response) => {
            console.log(response);
            event.target.reset();
        });        
    }
    
    render() {
        return(
            <Form onSubmit={this.handleSubmit.bind(this)}>
                <Form.Group controlId="BookName">
                    <Form.Label>Name</Form.Label>
                    <Form.Control type="text" placeholder="Book Name" id="BookName" onChange={this.handleChange} />
                </Form.Group>
                <Form.Group controlId="Price">
                    <Form.Label>Price</Form.Label>
                    <Form.Control type="number" step="0.01" id="Price" onChange={this.handleChange}/>
                </Form.Group>
                <Form.Group controlId="Category">
                    <Form.Label>Category</Form.Label>
                    <Form.Control type="text" placeholder="Category" id="Category" onChange={this.handleChange} />
                </Form.Group>
                <Form.Group controlId="Author">
                    <Form.Label>Author</Form.Label>
                    <Form.Control type="text" placeholder="Author Name" id="Author" onChange={this.handleChange} />
                </Form.Group>
                <Button variant="primary" type="submit">
                    Submit
                </Button>
            </Form>
        )
    }
}