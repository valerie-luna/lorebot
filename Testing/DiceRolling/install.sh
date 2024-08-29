#!/bin/bash
python -m venv $(pwd)/pythonvenv
cd grammarinator
$(pwd)/../pythonvenv/bin/pip install .
# todo: fix this i guess
sed -i 's/'-n', default=1, type=int/'-n', default=1, type=float/' $(pwd)/../pythonvenv/lib/python3.12/site-packages/grammarinator/generate.py