import React from 'react';

export default class ViewBook extends React.Component {
    render() {
        return(
            <div>
                <h2>this.props.bookName</h2>
                <p>this.props.author</p>
                <p>this.props.category</p>
                <p>this.props.price</p>
            </div>           
        )
    }
}