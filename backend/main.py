from flask import Flask, jsonify, request, send_file
from rutracker import Rutracker
import requests
import json

API_KEY = "" # api key last.fm

x = Rutracker('', '') # rutracker login and password

app = Flask(__name__)

@app.route('/')
def index():
    return "Hello, World!"

@app.route('/get_songs', methods=['GET'])
def get_tasks():
    req = requests.get("http://ws.audioscrobbler.com/2.0/?method=track.search&track=" + request.args.get('name') + "&api_key=" + API_KEY + "&format=json")
    if (len(request.args.get('name')) <= 0):
        return jsonify({'songs': []})
    songs = json.loads(req.content)["results"]["trackmatches"]["track"]
    if (len(songs) > 0):
        song = x.search(songs[0]["artist"] + " " + songs[0]["name"])
        result = []
        for val in song:
            result.append({"topic": val[1], "topic_id": str(val[2])})
        return jsonify({'songs': result})
    else:
        return jsonify({'songs': []})

@app.route('/get_torrent', methods=['GET'])
def get_torrent():
    x.get_torrent(request.args.get('id').replace("_", ""), path="torrent_files")
    return send_file("torrent_files/" + request.args.get('id').replace("_", "") + ".torrent")

if __name__ == '__main__':
    app.run(debug=True, host="0.0.0.0", port=1337)