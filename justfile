name := "PMLSQLite"

@_default:
    just --list

# Release the binary to Github
release ver msg:
    make clean prod
    rm -f *.zip
    7z a {{name}}_{{ver}}.zip ./dist/* README.md
    git tag -a v{{ver}} -m "{{msg}}"
    git push origin v{{ver}}
    gh release create -n "{{msg}}" v{{ver}} {{name}}_{{ver}}.zip

unrelease ver:
    gh release delete v{{ver}}
    git push origin --delete v{{ver}}
    git tag -d v{{ver}}
