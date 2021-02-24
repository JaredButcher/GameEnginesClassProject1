import flask
from flask_cors import CORS, cross_origin
import random

def createApp():
    app = flask.Flask(__name__)
    app.config['CORS_HEADERS'] = 'Content-Type'
    return app

random.seed()
app = createApp()
cors = CORS(app)
drops = {}

@app.route('/')
def home():
    return "Not found", 404

@app.route('/level', methods=['POST'])
@cross_origin()
def requestNetDrop():
    formEntries = [ entry.split('=') for entry in flask.request.form[None].split('&')]
    dropList = drops.get(formEntries[0][1], [])
    if len(dropList) > 0:
        drop = dropList.pop()
        return drop, 200
    return "F", 200

@app.route('/drop', methods=['POST'])
@cross_origin()
def sendNetDrop():
    formEntries = [ entry.split('=') for entry in flask.request.form[None].split('&')]
    dropList = drops.get(formEntries[0][1], [])
    dropList.append(formEntries[1][1])
    drops[formEntries[0][1]] = dropList
    return "Sucess", 200 

def run():
    app.run(debug=False, port=3493, host='0.0.0.0')


if __name__ == '__main__':
    run()
