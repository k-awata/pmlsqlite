CSPROJ := PMLSQLite.csproj
BIN := PMLSQLite.dll
VERS := E3D2.1 E3D3.1 UE4.0

DIST_DIR := dist
SRC := $(CSPROJ) $(wildcard *.cs)
VERDIRS := $(foreach ver,$(VERS),$(DIST_DIR)/$(ver)/$(BIN))
PML_DIR := pml
PMLJSON_DIR := pml-json
PML  := $(patsubst $(PML_DIR)/%,$(DIST_DIR)/%,$(filter-out %Test.pmlobj,$(wildcard $(PML_DIR)/*))) $(DIST_DIR)/json.pmlobj
TEST := $(patsubst $(PML_DIR)/%,$(DIST_DIR)/%,$(filter     %Test.pmlobj,$(wildcard $(PML_DIR)/*))) $(DIST_DIR)/jsonTest.pmlobj

.PHONY: clean prod dev licenses

dev: $(VERDIRS) $(PML) $(TEST)

prod: $(VERDIRS) $(PML) licenses

licenses:
	cp LICENSE $(DIST_DIR)/LICENSE
	nuget-license -i $(CSPROJ) -ignore "NETStandard.Library" > $(DIST_DIR)/THIRD_PARTY_NOTICES.txt

clean:
	rm -rf $(DIST_DIR) bin obj

$(DIST_DIR)/%/$(BIN): $(SRC)
	dotnet publish -o $(@D) -p:AvevaVersion="$*"

$(DIST_DIR)/%: $(PML_DIR)/%
	cp $(PML_DIR)/$* $(DIST_DIR)

$(DIST_DIR)/%: $(PMLJSON_DIR)/%
	cp $(PMLJSON_DIR)/$* $(DIST_DIR)
